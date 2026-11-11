using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem2D : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) // узнать что в скобках
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(ItemType.Banana);
            Destroy(gameObject);
        }
    }
}