using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone2D : MonoBehaviour
{
    public GameObject trapPrefab;
    private bool _hasPlayer = false;
    private bool _trapPlaced = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _hasPlayer = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _hasPlayer = false;
    }

    void Update()
    {
        if (_hasPlayer && !_trapPlaced && Input.GetKeyDown(KeyCode.E))
        {
            // ПРОВЕРКА: есть ли банан в рюкзаке?
            if (Inventory.Instance.HasItem(ItemType.Banana))
            {
                Inventory.Instance.RemoveItem(ItemType.Banana); // Забираем банан

                // Создаем ловушку
                GameObject trap = Instantiate(trapPrefab, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
                BananaTrap2D trapScript = trap.GetComponent<BananaTrap2D>();
                if (trapScript != null) trapScript.OnTrapTriggered += ResetZone;

                _trapPlaced = true;
                Debug.Log("Ловушка установлена! Банан потрачен.");
            }
            else
            {
                Debug.Log("У вас нет банана! Сначала найдите его.");
            }
        }
    }

    private void ResetZone()
    {
        _trapPlaced = false;
        Debug.Log("Зона свободна.");
    }
}