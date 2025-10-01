using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRecorder : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Recording")]
    public float recordTime = 3f;
    public float recordMinDistance = 0.05f;
    public float recordMinInterval = 0.06f; // 최소 기록 간격
    private float lastRecordTime = 0f;

    [Header("Rewind")]
    public float rewindSpeedMultiplier = 1.8f;

    [Header("Optional Visual")]
    public Material rewindMaterial;
    private Material originalMaterial;
    private Renderer rend;

    private Queue<(Vector3 pos, float time)> recordQueue = new Queue<(Vector3, float)>();
    private Stack<Vector3> positionStack = new Stack<Vector3>();

    private bool isRewinding = false;
    private Vector3 lastRecordedPos;

    // === 명령 큐 & 재생 제어 ===
    private Queue<Vector3> commandQueue = new Queue<Vector3>();
    private bool isPlayingCommands = false;

    [Header("Command Play")]
    public float commandStepDistance = 1.2f;   // 한 명령당 이동 거리
    public float commandMoveSpeed = 8f;        // 명령 재생 시 속도(유닛/초)

    // === 되감기 코루틴 ===
    private Coroutine rewindCo = null;
    private float rewindSpeed => moveSpeed * rewindSpeedMultiplier;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalMaterial = rend.material;
        lastRecordedPos = transform.position;
        lastRecordTime = Time.time;
    }

    void Update()
    {
        if (!isRewinding && !isPlayingCommands)
        {
            HandleMovementInput();
            RecordPositionIfNeeded();

            // 오래된 기록 제거 (최근 recordTime만 유지)
            while (recordQueue.Count > 0 && Time.time - recordQueue.Peek().time > recordTime)
                recordQueue.Dequeue();

            // Space → 명령 실행
            if (Input.GetKeyDown(KeyCode.Space) && commandQueue.Count > 0)
                StartCoroutine(PlayCommands());

            // R → 되감기
            if (Input.GetKeyDown(KeyCode.R))
                StartRewind();
        }
    }

    void HandleMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) commandQueue.Enqueue(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.S)) commandQueue.Enqueue(Vector3.back);
        if (Input.GetKeyDown(KeyCode.A)) commandQueue.Enqueue(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) commandQueue.Enqueue(Vector3.right);
    }

    IEnumerator PlayCommands()
    {
        isPlayingCommands = true;
        ForceRecordSnapshot();

        while (commandQueue.Count > 0)
        {
            Vector3 dir = commandQueue.Dequeue();
            yield return SmoothMove(dir); // 딱딱 → 부드럽게
        }

        isPlayingCommands = false;
        lastRecordedPos = transform.position;
    }

    IEnumerator SmoothMove(Vector3 dir)
    {
        Vector3 start = transform.position;
        Vector3 target = start + dir.normalized * commandStepDistance;

        float duration = commandStepDistance / commandMoveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smooth = Mathf.SmoothStep(0f, 1f, t); // 가속-감속 보간
            transform.position = Vector3.Lerp(start, target, smooth);

            RecordPositionIfNeeded_DuringMovement();
            TrimOldRecords();

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        ForceRecordSnapshot();
    }

    void RecordPositionIfNeeded()
    {
        bool farEnough = (transform.position - lastRecordedPos).sqrMagnitude >= recordMinDistance * recordMinDistance;
        bool timeEnough = (Time.time - lastRecordTime) >= recordMinInterval;

        if (farEnough || timeEnough)
        {
            recordQueue.Enqueue((transform.position, Time.time));
            lastRecordedPos = transform.position;
            lastRecordTime = Time.time;
        }
    }

    void RecordPositionIfNeeded_DuringMovement()
    {
        bool farEnough = (transform.position - lastRecordedPos).sqrMagnitude >= recordMinDistance * recordMinDistance * 0.5f;
        bool timeEnough = (Time.time - lastRecordTime) >= recordMinInterval * 0.5f;

        if (farEnough || timeEnough)
        {
            recordQueue.Enqueue((transform.position, Time.time));
            lastRecordedPos = transform.position;
            lastRecordTime = Time.time;
        }
    }

    void ForceRecordSnapshot()
    {
        recordQueue.Enqueue((transform.position, Time.time));
        lastRecordedPos = transform.position;
        lastRecordTime = Time.time;
    }

    void TrimOldRecords()
    {
        float cutoff = Time.time - recordTime;
        while (recordQueue.Count > 0 && recordQueue.Peek().time < cutoff)
            recordQueue.Dequeue();
    }

    void StartRewind()
    {
        if (rewindCo != null) StopCoroutine(rewindCo);

        isRewinding = true;
        isPlayingCommands = false;
        commandQueue.Clear();

        if (rend != null && rewindMaterial != null) rend.material = rewindMaterial;

        ForceRecordSnapshot();
        TrimOldRecords();

        rewindCo = StartCoroutine(RewindCoroutine());
    }

    IEnumerator RewindCoroutine()
    {
        float cutoff = Time.time - recordTime;
        var arr = recordQueue.ToArray();

        List<Vector3> path = new List<Vector3>(arr.Length + 1);
        path.Add(transform.position);

        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i].time >= cutoff)
            {
                if (path.Count == 0 || (path[path.Count - 1] - arr[i].pos).sqrMagnitude > 0.00001f)
                    path.Add(arr[i].pos);
            }
        }

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 target = path[i];
            while ((transform.position - target).sqrMagnitude > 0.0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, rewindSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = target;
        }

        StopRewind();
    }

    void StopRewind()
    {
        if (rewindCo != null) { StopCoroutine(rewindCo); rewindCo = null; }

        isRewinding = false;
        positionStack.Clear();
        lastRecordedPos = transform.position;
        lastRecordTime = Time.time;

        if (rend != null && originalMaterial != null) rend.material = originalMaterial;
    }
}
