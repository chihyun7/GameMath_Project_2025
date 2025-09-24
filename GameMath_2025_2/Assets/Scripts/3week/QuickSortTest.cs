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

    // ���� �迭�� �����ϴ� �Լ�
    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();

        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000); // 0~9999 ������ ����
        }

        return arr;
    }

    // �� ���� ���� �Լ�
    public static void StartQuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            // �迭�� ��Ƽ�Ŵ��ϰ� �ǹ� ��ġ ��ȯ
            int pivotIndex = Partition(arr, low, high);

            // �ǹ� �������� ���� �κ� ����
            StartQuickSort(arr, low, pivotIndex - 1);

            // �ǹ� �������� ������ �κ� ����
            StartQuickSort(arr, pivotIndex + 1, high);
        }
    }

    // ��Ƽ�� �Լ� (�ǹ��� �������� �迭�� ����)
    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high]; // ������ ��Ҹ� �ǹ����� ����
        int i = (low - 1);      // i�� ���� ���� �ε���

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                // i��°�� j��° ��Ҹ� ��ȯ
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        // �ǹ��� �߰� ��ġ�� �̵�
        int temp2 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp2;

        return i + 1; // �ǹ��� ���� ��ġ ��ȯ
    }
}

