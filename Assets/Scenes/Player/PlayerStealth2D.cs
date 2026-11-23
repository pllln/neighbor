using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealth2D : MonoBehaviour
{
    public bool IsHidden { get; private set; }
    private int _hideZoneCount = 0;
    private SpriteRenderer _sprite;

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void EnterHideZone()
    {
        _hideZoneCount++;
        IsHidden = true;
        if (_sprite != null)
            _sprite.color = new Color(1, 1, 1, 0.4f);
    }

    public void ExitHideZone()
    {
        _hideZoneCount--;
        if (_hideZoneCount <= 0)
        {
            _hideZoneCount = 0;
            IsHidden = false;
            if (_sprite != null)
                _sprite.color = Color.white;
        }
    }
}