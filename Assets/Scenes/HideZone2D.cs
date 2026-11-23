using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideZone2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStealth2D>().EnterHideZone();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStealth2D>().ExitHideZone();
        }
    }
}