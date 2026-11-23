using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseTrap2D : MonoBehaviour
{
    public event Action OnTrapTriggered;
    public int scoreValue = 20;
    public float reactTime = 2f;
    public bool destroyOnTrigger = true;
    protected bool _triggered = false;

    protected virtual void TriggerTrap(NeighborAI2D neighbor)
    {
        if (_triggered) return;
        _triggered = true;

        if (neighbor != null) neighbor.ReactToTrap(reactTime);
        GameManager.Instance?.AddScore(scoreValue);
        OnTrapTriggered?.Invoke();

        if (destroyOnTrigger) Destroy(gameObject);
    }

    public virtual void TriggerManualTrap()
    {
        if (_triggered) return;
        _triggered = true;

        GameManager.Instance?.AddScore(scoreValue);
        OnTrapTriggered?.Invoke();

        if (destroyOnTrigger) Destroy(gameObject);
    }
    public void ForceTriggerEvent()
    {
        OnTrapTriggered?.Invoke();
    }
}