using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealth2D : MonoBehaviour
{
    public bool IsHidden { get; private set; }
    private int _hideZoneCount = 0;
    private SpriteRenderer _sprite;
    private Collider2D _collider; // Ссылка на коллайдер

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }

    public void EnterHideZone()
    {
        _hideZoneCount++;
        IsHidden = _hideZoneCount > 0;

        // Визуал: делаем полупрозрачным
        _sprite.color = new Color(1, 1, 1, 0.4f);

        // ФИЗИКА: отключаем коллайдер, чтобы сосед не толкал игрока
        if (_collider != null) _collider.enabled = false;
    }

    public void ExitHideZone()
    {
        _hideZoneCount--;
        if (_hideZoneCount <= 0)
        {
            _hideZoneCount = 0;
            IsHidden = false;

            _sprite.color = Color.white;

            // ФИЗИКА: включаем коллайдер обратно
            if (_collider != null) _collider.enabled = true;
        }
    }
}