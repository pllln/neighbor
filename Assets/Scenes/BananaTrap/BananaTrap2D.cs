using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Нужно для Action

public class BananaTrap2D : MonoBehaviour
{
    // Событие, которое сообщит зоне, что ловушка сработала
    public event Action OnTrapTriggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Neighbor"))
        {
            other.GetComponent<NeighborAI2D>().ReactToTrap();

            // Вызываем событие перед удалением
            OnTrapTriggered?.Invoke();

            Destroy(gameObject);
        }
    }
}