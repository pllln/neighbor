using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborAI2D : MonoBehaviour
{
    [Header("Маршрут")]
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4.5f;

    [Header("Поимка")]
    public Transform player;
    public float viewDistance = 6f;
    public float catchDistance = 0.8f;
    public LayerMask obstacleMask;

    [Header("Цикл")]
    public float patrolCycleDuration = 15f;   // сколько ходит, прежде чем уйти

    [Header("Тайминги")]
    public float loseTime = 3f;       // сколько помнит игрока после потери из виду
    public float reactDuration = 2f;  // пауза после ловушки

    private int _currentWP = 0;
    private Rigidbody2D _rb;
    private PlayerStealth2D _playerStealth;
    private bool _isChasing = false;
    private bool _isCaught = false;

    private float _loseTimer = 0f;
    private float _reactTimer = 0f;
    private float _patrolTimer = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (player != null)
            _playerStealth = player.GetComponent<PlayerStealth2D>();
    }

    void Update()
    {
        if (player == null || _isCaught) return;

        // Пауза после реакции на ловушку
        if (_reactTimer > 0f)
        {
            _reactTimer -= Time.deltaTime;
            return;
        }

        float distanceToPlayer = Vector2.Distance(_rb.position, player.position);
        bool isHidden = _playerStealth != null && _playerStealth.IsHidden;

        // Может ли сосед видеть игрока
        bool canSeePlayer = distanceToPlayer < viewDistance && !isHidden;

        // Проверка стен
        if (canSeePlayer)
        {
            Vector2 dir = ((Vector2)player.position - _rb.position).normalized;
            var hit = Physics2D.Raycast(_rb.position, dir, distanceToPlayer, obstacleMask);
            if (hit.collider != null) canSeePlayer = false;
        }

        // Логика погони с таймером забывания
        if (canSeePlayer)
        {
            _isChasing = true;
            _loseTimer = 0f;
        }
        else if (_isChasing)
        {
            _loseTimer += Time.deltaTime;
            if (_loseTimer >= loseTime)
            {
                _isChasing = false;
                _loseTimer = 0f;
            }
        }

        if (_isChasing) ChasePlayer(distanceToPlayer);
        else Patrol();

        // Уход соседа после патруля
        if (!_isChasing && _reactTimer <= 0f)
        {
            _patrolTimer += Time.deltaTime;
            if (_patrolTimer >= patrolCycleDuration)
            {
                _patrolTimer = 0f;
                gameObject.SetActive(false);
                TimerManager.Instance?.OnNeighborLeftRoom();
            }
        }
        else
        {
            _patrolTimer = 0f;
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Vector2 target = waypoints[_currentWP].position;
        _rb.MovePosition(Vector2.MoveTowards(_rb.position, target, patrolSpeed * Time.deltaTime));

        if (Vector2.Distance(_rb.position, target) < 0.1f)
            _currentWP = (_currentWP + 1) % waypoints.Length;
    }

    void ChasePlayer(float distance)
    {
        bool isHidden = _playerStealth != null && _playerStealth.IsHidden;

        if (!isHidden)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, player.position, chaseSpeed * Time.deltaTime));
        }

        if (distance < catchDistance && !isHidden)
        {
            CatchPlayer();
        }
    }

    void CatchPlayer()
    {
        _isCaught = true;
        Debug.Log("Усп! Вас засекли");
        GameManager.Instance?.PlayerCaught();
        Invoke(nameof(AfterCatch), 1.5f);
    }

    void AfterCatch()
    {
        gameObject.SetActive(false);
        TimerManager.Instance?.OnNeighborLeftRoom();
    }

    // Теперь метод принимает время паузы (duration) от ловушки
    public void ReactToTrap(float duration)
    {
        Debug.Log("Сосед: ***! Что это было?!");
        _isChasing = false;
        _reactTimer = duration;
    }

    public void EnterRoom()
    {
        _isCaught = false;
        _isChasing = false;
        _reactTimer = 0f;
        _loseTimer = 0f;
        _patrolTimer = 0f;
        _currentWP = 0;

        // Усложнение: с каждым циклом сосед ходит быстрее
        if (GameManager.Instance != null)
        {
            int cycle = GameManager.Instance.currentCycle;
            patrolSpeed = 2f + (cycle * 0.3f); // 1 цикл: 2.3, 2 цикл: 2.6, 3 цикл: 2.9 и т.д.
            chaseSpeed = 4.5f + (cycle * 0.4f); // Бегает еще быстрее
        }

        Debug.Log("Сосед вошёл в комнату. Цикл: " + GameManager.Instance.currentCycle);
    }

    public void ResetAI()
    {
        _isCaught = false;
        _isChasing = false;
        _reactTimer = 0f;
        _loseTimer = 0f;
        _patrolTimer = 0f;
        _currentWP = 0;
    }
}