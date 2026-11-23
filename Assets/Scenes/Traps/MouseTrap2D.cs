using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap2D : BaseTrap2D
{
    public bool isTrapPlaced = false;
    public GameObject trapVisual;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Neighbor") && isTrapPlaced && !_triggered)
        {
            _triggered = true;
            var neighbor = other.GetComponent<NeighborAI2D>();

            if (neighbor != null) neighbor.ReactToTrap(reactTime);

            Debug.Log("Сосед: ААА! Мышь!!!");
            GameManager.Instance?.AddScore(scoreValue);

            // Жестко удаляем мышь
            if (trapVisual != null) Destroy(trapVisual);

            isTrapPlaced = false;
            _triggered = false;
        }
    }
}