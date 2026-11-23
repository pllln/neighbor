using UnityEngine;

public class BananaTrap2D : BaseTrap2D
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Neighbor"))
        {
            var neighbor = other.GetComponent<NeighborAI2D>();
            TriggerTrap(neighbor);
        }
    }
}