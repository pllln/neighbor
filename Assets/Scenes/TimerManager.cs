using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [Header("Настройки")]
    public float awayDuration = 40f;
    public float warningThreshold = 10f;

    [Header("Ссылки")]
    public NeighborAI2D neighbor;

    private float _timer;
    private bool _warningShown = false;
    private bool _cycleActive = false;

    void Awake() => Instance = this;

    void Start()
    {
        StartNewCycle();
    }

    public void StartNewCycle()
    {
        if (neighbor == null) return;

        // С каждым циклом время на подготовку уменьшается
        if (GameManager.Instance != null)
        {
            int cycle = GameManager.Instance.currentCycle;
            // 1 цикл: 40 сек, 2 цикл: 35 сек, 3 цикл: 30 сек...
            awayDuration = Mathf.Max(20f, 40f - (cycle - 1) * 5f);
        }

        _timer = awayDuration;
        _warningShown = false;
        _cycleActive = true;

        neighbor.gameObject.SetActive(false);
        neighbor.ResetAI();

        HUDController.Instance?.UpdateTimer(1f);
        HUDController.Instance?.ShowWarning("Сосед ушёл. Готовь ловушку");

        // Спавним предметы заново для нового цикла
        ItemSpawner2D.Instance?.SpawnAllItems();

        // Сбрасываем все зоны на сцене (чтобы можно было ставить ловушки заново)
        TrapZone2D[] allZones = FindObjectsOfType<TrapZone2D>();
        foreach (var zone in allZones)
        {
            zone.ForceReset();
        }
    }

    void Update()
    {
        if (!_cycleActive) return;

        _timer -= Time.deltaTime;

        if (_timer < 0f) _timer = 0f;
        HUDController.Instance?.UpdateTimer(_timer / awayDuration);

        if (_timer < warningThreshold && !_warningShown && _timer > 0f)
        {
            _warningShown = true;
            HUDController.Instance?.ShowWarning("Сосед возвращается!");
            Debug.Log("Сосед близко!");
        }

        if (_timer <= 0f)
        {
            _cycleActive = false;
            neighbor.gameObject.SetActive(true);
            neighbor.EnterRoom();
            Debug.Log("Сосед вернулся");
        }
    }

    // Вызывается из NeighborAI2D, когда сосед закончил цикл и уходит снова
    public void OnNeighborLeftRoom()
    {
        GameManager.Instance?.OnCycleEnd();

        if (GameManager.Instance != null && GameManager.Instance.cyclesRemaining > 0
            && GameManager.Instance.score < GameManager.Instance.targetScore)
        {
            StartNewCycle();
        }
    }
}
