using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject itemPrefab;      // 아이템 버튼 프리팹
    public Transform contentPanel;     // ScrollView Content
    public InputField searchInput;     // 검색 입력창
    public Button linearButton;        // 선형 탐색 버튼
    public Button binaryButton;        // 이진 탐색 버튼

    private List<GameObject> itemObjects = new List<GameObject>();
    private List<string> itemNames = new List<string>();

    void Start()
    {
        // 100개 아이템 생성
        for (int i = 0; i < 100; i++)
        {
            string itemName = "Item_" + i.ToString("D2");
            itemNames.Add(itemName);

            GameObject newItem = Instantiate(itemPrefab, contentPanel);
            newItem.GetComponentInChildren<Text>().text = itemName;
            itemObjects.Add(newItem);
        }

        // 버튼 연결
        linearButton.onClick.AddListener(() => LinearSearch(searchInput.text));
        binaryButton.onClick.AddListener(() => BinarySearch(searchInput.text));
    }

    // 모든 아이템 숨기기
    private void HideAllItems()
    {
        foreach (var item in itemObjects)
        {
            item.SetActive(false);
        }
    }

    // 선형 탐색
    private void LinearSearch(string target)
    {
        HideAllItems();
        for (int i = 0; i < itemNames.Count; i++)
        {
            if (itemNames[i] == target)
            {
                itemObjects[i].SetActive(true);
                return;
            }
        }
        Debug.Log("아이템 없음 (Linear Search)");
    }

    // 이진 탐색
    private void BinarySearch(string target)
    {
        HideAllItems();

        int left = 0;
        int right = itemNames.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = string.Compare(itemNames[mid], target);

            if (compare == 0)
            {
                itemObjects[mid].SetActive(true);
                return;
            }
            else if (compare < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        Debug.Log("아이템 없음 (Binary Search)");
    }
}
