using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeighborAI2D : MonoBehaviour
{
    [Header("Маршрут патруля")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4.5f;

    [Header("Зрение и поимка")]
    public Transform player;
    public float viewDistance = 6f;
    public float catchDistance = 0.8f;

    private int _currentWP = 0;
    private Rigidbody2D _rb;
    private bool _isChasing = false;
    private bool _isCaught = false;

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        if (player == null || _isCaught) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Проверяем, спрятан ли игрок
        bool isHidden = player.GetComponent<PlayerStealth2D>() != null && player.GetComponent<PlayerStealth2D>().IsHidden;

        // Сосед видит игрока, ТОЛЬКО если он не спрятан
        bool canSeePlayer = distanceToPlayer < viewDistance && !isHidden;

        if (canSeePlayer)
        {
            _isChasing = true;
        }
        else
        {
            _isChasing = false;
        }

        if (_isChasing)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Vector2 target = waypoints[_currentWP].position;
        _rb.MovePosition(Vector2.MoveTowards(_rb.position, target, patrolSpeed * Time.deltaTime));

        if (Vector2.Distance(_rb.position, target) < 0.1f)
        {
            _currentWP = (_currentWP + 1) % waypoints.Length;
        }
    }

    void ChasePlayer(float distance)
    {
        _rb.MovePosition(Vector2.MoveTowards(_rb.position, player.position, chaseSpeed * Time.deltaTime));

        // Ловим, ТОЛЬКО если игрок не спрятан и мы близко
        bool isHidden = player.GetComponent<PlayerStealth2D>().IsHidden;

        if (distance < catchDistance && !isHidden)
        {
            CatchPlayer();
        }
    }

    void CatchPlayer()
    {
        _isCaught = true;
        Debug.Log("Сосед поймал игрока! Перезапуск...");

        // Перезапускаем текущую сцену через 1.5 секунды
        Invoke(nameof(RestartLevel), 1.5f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReactToTrap()
    {
        Debug.Log("Сосед: Ай! Что это было?!");
        _isChasing = false;
    }
}