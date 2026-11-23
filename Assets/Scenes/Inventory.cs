//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<ItemType> items = new List<ItemType>();

    void Awake()
    {
        Instance = this;
    }
   
    public int maxSlots = 4;

    public bool AddItem(ItemType item)
    {
        if (items.Count >= maxSlots) return false;
        items.Add(item);
        Debug.Log("В рюкзаке: " + item);
        return true;
    }

    public bool HasItem(ItemType item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(ItemType item)
    {
        items.Remove(item);
    }
}
