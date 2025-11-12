using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject pathPrefab;

    int[,] map;
    List<Vector2Int> pathResult = new List<Vector2Int>();

    private void Start()
    {
        GenerateRandomMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateRandomMap();

        if (Input.GetKeyDown(KeyCode.R))
            ShowPath();
    }

    void GenerateRandomMap()
    {
        Clear();

        do
        {
            CreateRandomWalls();
        }
        while (!BFS_FindPath());  // 길이 없으면 다시 생성

        Draw();
    }

    void CreateRandomWalls()
    {
        map = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = Random.Range(0, 100) < 65 ? 1 : 0;  // ✅ 무작위 벽 생성 (65% 벽)

        map[1, 1] = 0;                        // 시작점 강제 길
        map[width - 2, height - 2] = 0;       // 목표 위치 강제 길
    }

    bool BFS_FindPath()
    {
        pathResult.Clear();

        Vector2Int start = new Vector2Int(1, 1);
        Vector2Int goal = new Vector2Int(width - 2, height - 2);

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        bool[,] visited = new bool[width, height];
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

        q.Enqueue(start);
        visited[start.x, start.y] = true;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (q.Count > 0)
        {
            Vector2Int cur = q.Dequeue();

            if (cur == goal)
            {
                Vector2Int trace = goal;
                while (trace != start)
                {
                    pathResult.Add(trace);
                    trace = parent[trace];
                }
                return true; // ✅ 탈출 가능!
            }

            foreach (var d in dirs)
            {
                Vector2Int next = cur + d;

                if (next.x <= 0 || next.y <= 0 || next.x >= width || next.y >= height)
                    continue;
                if (map[next.x, next.y] == 1)
                    continue;
                if (visited[next.x, next.y])
                    continue;

                visited[next.x, next.y] = true;
                parent[next] = cur;
                q.Enqueue(next);
            }
        }

        return false;
    }

    void Draw()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Instantiate(map[x, y] == 1 ? wallPrefab : floorPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
    }

    void ShowPath()
    {
        foreach (var p in pathResult)
            Instantiate(pathPrefab, new Vector3(p.x, 0.3f, p.y), Quaternion.identity, transform);
    }

    void Clear()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);
    }
}
