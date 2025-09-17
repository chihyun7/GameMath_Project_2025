using System.Diagnostics;
using UnityEngine;

public class SortManager : MonoBehaviour
{
    void Start()
    {
        int[] sizes = { 100, 1000, 10000 };

        foreach (int size in sizes)
        {
            int[] arr1 = GenerateRandomArray(size);
            int[] arr2 = (int[])arr1.Clone();
            int[] arr3 = (int[])arr1.Clone();

            // ���� ����
            Stopwatch sw = Stopwatch.StartNew();
            BubbleSortTest.StartBubbleSort(arr1);
            sw.Stop();
            UnityEngine.Debug.Log($"BubbleSort {size}��: {sw.ElapsedMilliseconds} ms");

            // ���� ����
            sw = Stopwatch.StartNew();
            SelectionSortTest.StartSelectionSort(arr2);
            sw.Stop();
            UnityEngine.Debug.Log($"SelectionSort {size}��: {sw.ElapsedMilliseconds} ms");

            // �� ����
            sw = Stopwatch.StartNew();
            QuickSortTest.StartQuickSort(arr3, 0, arr3.Length - 1);
            sw.Stop();
            UnityEngine.Debug.Log($"QuickSort {size}��: {sw.ElapsedMilliseconds} ms");
        }
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 100000);
        }
        return arr;
    }
}
