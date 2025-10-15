using UnityEngine;
using System.Collections.Generic;

public class TurnBattleSystem : MonoBehaviour
{
    // 유닛의 정보를 담는 내부 클래스
    private class Unit
    {
        public string Name;
        public int Speed;
        public int TurnCost;     // *** 변경점 *** 스피드에 반비례하는 턴 비용 (낮을수록 좋음)
        public int NextTurnTick; // 다음 턴이 돌아오는 시점 (이 값이 가장 낮은 유닛이 행동함)

        public Unit(string name, int speed)
        {
            Name = name;
            Speed = speed;

            // *** 변경점 *** 스피드가 높을수록 턴 비용(TurnCost)이 낮아지도록 설정합니다.
            // 예를 들어 기준값 20에서 스피드를 빼서 비용을 계산합니다.
            // 도적(12) -> 비용 8  | 전사(5) -> 비용 15
            TurnCost = 20 - Speed;

            NextTurnTick = 0;
        }
    }

    // --- 턴 관리 및 힙(Heap) 자료구조 ---

    private List<Unit> turnHeap = new List<Unit>(); // 우선순위 큐 역할을 할 리스트 (최소 힙)
    private List<Unit> initialUnits = new List<Unit>(); // 첫 4턴을 위한 랜덤 리스트
    private int turnCount = 1;

    void Start()
    {
        // 1. 초기 유닛 생성
        List<Unit> unitsToShuffle = new List<Unit>
        {
            new Unit("전사", 5),
            new Unit("마법사", 7),
            new Unit("궁수", 10),
            new Unit("도적", 12)
        };

        // 2. 첫 4턴 순서를 랜덤으로 섞기
        System.Random rng = new System.Random();
        while (unitsToShuffle.Count > 0)
        {
            int k = rng.Next(unitsToShuffle.Count);
            initialUnits.Add(unitsToShuffle[k]);
            unitsToShuffle.RemoveAt(k);
        }

        Debug.Log("턴제 전투 시뮬레이션을 시작합니다. 스페이스바를 눌러 턴을 진행하세요.");
        Debug.Log("참고: 첫 4턴은 모든 유닛이 한 번씩 랜덤 순서로 행동합니다.");
    }

    void Update()
    {
        // 스페이스바를 누를 때마다 다음 턴 진행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessNextTurn();
        }
    }

    // 다음 턴을 진행하는 메인 함수
    void ProcessNextTurn()
    {
        Unit currentUnit;

        // 초기 4턴 단계인지 확인
        if (initialUnits.Count > 0)
        {
            currentUnit = initialUnits[0];
            initialUnits.RemoveAt(0);

            Debug.Log($"<color=yellow>{turnCount}턴</color> / {currentUnit.Name}의 턴입니다. (초기 턴)");

            // 행동 후, 자신의 턴 비용만큼 쿨타임을 계산하여 우선순위 큐에 추가
            currentUnit.NextTurnTick = currentUnit.TurnCost;
            Enqueue(currentUnit);
        }
        // 5턴부터는 우선순위 큐를 사용
        else
        {
            if (turnHeap.Count == 0) return;

            // 쿨타임이 가장 짧게 남은 유닛을 꺼냄 (Dequeue)
            currentUnit = Dequeue();

            Debug.Log($"{turnCount}턴 (Tick: {currentUnit.NextTurnTick}) / {currentUnit.Name}의 턴입니다.");

            // *** 변경점 *** 행동 후, Speed가 아닌 TurnCost를 더해 다음 턴을 계산
            currentUnit.NextTurnTick += currentUnit.TurnCost;
            Enqueue(currentUnit);
        }

        turnCount++;
    }

    // --- 우선순위 큐 (최소 힙) 메소드 ---

    private void Enqueue(Unit unit)
    {
        turnHeap.Add(unit);
        HeapifyUp(turnHeap.Count - 1);
    }

    private Unit Dequeue()
    {
        Unit rootUnit = turnHeap[0];
        turnHeap[0] = turnHeap[^1];
        turnHeap.RemoveAt(turnHeap.Count - 1);
        if (turnHeap.Count > 1)
        {
            HeapifyDown(0);
        }
        return rootUnit;
    }

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (turnHeap[i].NextTurnTick < turnHeap[parent].NextTurnTick)
            {
                (turnHeap[i], turnHeap[parent]) = (turnHeap[parent], turnHeap[i]);
                i = parent;
            }
            else
            {
                break;
            }
        }
    }

    private void HeapifyDown(int i)
    {
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < turnHeap.Count && turnHeap[left].NextTurnTick < turnHeap[smallest].NextTurnTick)
            {
                smallest = left;
            }
            if (right < turnHeap.Count && turnHeap[right].NextTurnTick < turnHeap[smallest].NextTurnTick)
            {
                smallest = right;
            }

            if (smallest == i) break;

            (turnHeap[i], turnHeap[smallest]) = (turnHeap[smallest], turnHeap[i]);
            i = smallest;
        }
    }
}