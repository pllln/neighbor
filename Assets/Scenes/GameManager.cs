using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Баланс")]
    public int targetScore = 100;
    public int maxLives = 3;
    public int totalCycles = 5;

    [Header("Состояние (только для чтения)")]
    public int score = 0;
    public int lives = 3;
    public int cyclesRemaining = 5;

    public int currentCycle = 1;

    private bool _gameEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        lives = maxLives;
        cyclesRemaining = totalCycles;
    }

    void Start()
    {
        HUDController.Instance?.UpdateScore(score);
        HUDController.Instance?.UpdateLives(lives);
        currentCycle = 1;
    }

    public void AddScore(int amount)
    {
        if (_gameEnded) return;
        score += amount;
        HUDController.Instance?.UpdateScore(score);
        HUDController.Instance?.ShowFloatingText("+" + amount);

        if (score >= targetScore) Win();
    }

    public void PlayerCaught()
    {
        if (_gameEnded) return;
        lives--;
        HUDController.Instance?.UpdateLives(lives);
        HUDController.Instance?.ShowWarning("Поймали! Осталось жизней: " + lives);

        if (lives <= 0) Lose();
    }

    public void OnCycleEnd()
    {
        if (_gameEnded) return;

        cyclesRemaining--;
        currentCycle++; // Увеличиваем номер цикла

        if (cyclesRemaining <= 0 && score < targetScore) Lose();
    }

    void Win()
    {
        _gameEnded = true;
        Debug.Log("ПОБЕДА! Очки: " + score);
        HUDController.Instance?.ShowEndScreen(true);
    }

    void Lose()
    {
        _gameEnded = true;
        Debug.Log("ПОРАЖЕНИЕ. Очки: " + score);
        HUDController.Instance?.ShowEndScreen(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}