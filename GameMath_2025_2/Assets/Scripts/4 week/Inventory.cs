using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  public List<Item> items = new List<Item>();

    void start()
    {

        items.Add(new Item("Sword"));
        items.Add(new Item("Shield"));
        items.Add(new Item("Potion"));

        Item found = FindItem("Potion");

        if (found != null)
            Debug.Log("ã�� ������: "  +  found.quantity);
        else
            Debug.Log("�������� ã�� �� �����ϴ�.");       
    }


    public Item FindItem(string _ItemName)
    {
        foreach (var item in items)
        {
            if (item.itemName == _ItemName)
                return item;
        }
        return null;
    }
}
