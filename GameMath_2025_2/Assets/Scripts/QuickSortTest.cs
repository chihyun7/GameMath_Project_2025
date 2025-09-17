using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class QuickSortTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    // Start is called before the first frame update
  
    public void StartQuickSort()
    {
        int[] data = GenerateRandomArray(10000);
        
        Stopwatch sw = new Stopwatch();
        sw.Reset();
        sw.Start();
        StartQuickSort(data, 0, data.Length - 1);
        sw.Stop();
        long selectionTime = sw.ElapsedMilliseconds;

        string result = "QuickSort : " + selectionTime + " ms";
        UnityEngine.Debug.Log(result);

        if (resultText != null) resultText.text = result;


    }

    // 정수 배열을 생성하는 함수
    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000); // 0~9999 사이의 숫자
        }

        return arr;
    }

    // 퀵 정렬 메인 함수
    public static void StartQuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            // 배열을 파티셔닝하고 피벗 위치 반환
            int pivotIndex = Partition(arr, low, high);

            // 피벗 기준으로 왼쪽 부분 정렬
            StartQuickSort(arr, low, pivotIndex - 1);

            // 피벗 기준으로 오른쪽 부분 정렬
            StartQuickSort(arr, pivotIndex + 1, high);
        }
    }

    // 파티션 함수 (피벗을 기준으로 배열을 나눔)
    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high]; // 마지막 요소를 피벗으로 설정
        int i = (low - 1);      // i는 작은 값의 인덱스

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                // i번째와 j번째 요소를 교환
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // 피벗을 중간 위치로 이동
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1; // 피벗의 최종 위치 반환
    }
}

