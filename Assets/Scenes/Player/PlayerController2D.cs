using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D _rb;
    private float _moveInput;

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // Двигаемся только по X, чтобы не взлетать
        _rb.MovePosition(_rb.position + new Vector2(_moveInput * moveSpeed * Time.fixedDeltaTime, 0f));
    }
}