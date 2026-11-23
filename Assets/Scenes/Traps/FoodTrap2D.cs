using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrap2D : BaseTrap2D
{
    // Мы убрали проверку _wasEaten, чтобы еда реагировала стабильно
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Neighbor") && !_triggered)
        {
            _triggered = true;
            var neighbor = other.GetComponent<NeighborAI2D>();

            if (neighbor != null) neighbor.ReactToTrap(reactTime);

            Debug.Log("Сосед злится: Кто съел мою вкусняшку?!");

            ForceTriggerEvent();

            if (destroyOnTrigger) Destroy(gameObject);
        }
    }

    // Этот метод вызывается из TrapZone, когда игрок нажимает E
    public void EatFood()
    {
        Debug.Log("Игрок съел вкусняшку (или приготовился)!");
        // Начисляем очки сразу
        GameManager.Instance?.AddScore(scoreValue);
    }
}