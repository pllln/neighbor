using UnityEngine;

public class PickupItem2D : MonoBehaviour
{
    public ItemType itemType = ItemType.Banana; // Поле ItemType

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Inventory.Instance.AddItem(itemType))
            Destroy(gameObject);
        else
            Debug.Log("Рюкзак полон");
    }
}