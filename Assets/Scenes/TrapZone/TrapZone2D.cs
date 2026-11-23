using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone2D : MonoBehaviour
{
    public ItemType requiredItem = ItemType.None;
    public GameObject trapPrefab;
    public Vector2 spawnOffset = new Vector2(0, 0);

    [Header("Зона еды")]
    public bool isEatingZone = false;
    public FoodTrap2D foodOnTable;

    private bool _hasPlayer = false;
    private bool _trapPlaced = false;
    private GameObject _currentTrap;

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) _hasPlayer = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) _hasPlayer = false; }

    void Update()
    {
        if (!_hasPlayer || _trapPlaced) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isEatingZone && foodOnTable != null)
            {
                foodOnTable.EatFood();
                _trapPlaced = true;
                return;
            }

            if (requiredItem != ItemType.None && !Inventory.Instance.HasItem(requiredItem)) return;
            if (requiredItem != ItemType.None) Inventory.Instance.RemoveItem(requiredItem);

            if (trapPrefab != null)
            {
                // Жестко спавним ловушку (мышь появится на 1.5 метра выше зоны, чтобы не задеть игрока)
                Vector2 finalPos = (Vector2)transform.position + spawnOffset;
                _currentTrap = Instantiate(trapPrefab, finalPos, Quaternion.identity);

                var pickup = _currentTrap.GetComponent<PickupItem2D>();
                if (pickup != null) pickup.enabled = false;

                var mouseTrap = GetComponent<MouseTrap2D>();
                if (mouseTrap != null)
                {
                    mouseTrap.isTrapPlaced = true;
                    mouseTrap.trapVisual = _currentTrap;
                }
            }

            _trapPlaced = true;
            Debug.Log("Ловушка установлена!");
        }
    }

    public void ForceReset()
    {
        _trapPlaced = false;
        var mouseTrap = GetComponent<MouseTrap2D>();
        if (mouseTrap != null) mouseTrap.isTrapPlaced = false;
    }
}