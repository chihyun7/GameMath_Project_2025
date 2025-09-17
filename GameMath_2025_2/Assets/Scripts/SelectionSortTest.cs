using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionSortTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    // Start is called before the first frame update
    public void StartSelectionSort()
    {
        int[] data = GenerateRandomArray(10000);
        
       Stopwatch sw =new Stopwatch();
        sw.Reset();
        sw.Start();
        StartSelectionSort(data);
        sw.Stop();
        long selectionTime = sw.ElapsedMilliseconds;

        string result = "SelectionSort : " + selectionTime + " ms";
        UnityEngine.Debug.Log(result);

        if(resultText != null) resultText.text = result;

    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random rand = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = rand.Next(0, 10000); // Random integers between 0 and 999
        }
        return arr;
    }

    public static void StartSelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            // Find the minimum element in unsorted array
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }
            
            
                int temp = arr[minIndex];
                arr[minIndex] = arr[i];
                arr[i] = temp;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
