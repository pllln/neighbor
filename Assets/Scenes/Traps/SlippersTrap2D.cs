using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlippersTrap2D : BaseTrap2D
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Neighbor"))
        {
            var neighbor = other.GetComponent<NeighborAI2D>();
            TriggerTrap(neighbor);
            Debug.Log("Сосед: Где мои тапки?!");
        }
    }
}