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

            // 버블 정렬
            Stopwatch sw = Stopwatch.StartNew();
            BubbleSortTest.StartBubbleSort(arr1);
            sw.Stop();
            UnityEngine.Debug.Log($"BubbleSort {size}개: {sw.ElapsedMilliseconds} ms");

            // 선택 정렬
            sw = Stopwatch.StartNew();
            SelectionSortTest.StartSelectionSort(arr2);
            sw.Stop();
            UnityEngine.Debug.Log($"SelectionSort {size}개: {sw.ElapsedMilliseconds} ms");

            // 퀵 정렬
            sw = Stopwatch.StartNew();
            QuickSortTest.StartQuickSort(arr3, 0, arr3.Length - 1);
            sw.Stop();
            UnityEngine.Debug.Log($"QuickSort {size}개: {sw.ElapsedMilliseconds} ms");
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
