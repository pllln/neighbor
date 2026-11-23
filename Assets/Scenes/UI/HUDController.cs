using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI warningText;
    public Image timerBar;        // Image с Type = Filled, Fill Method = Horizontal
    public GameObject endScreen;
    public TextMeshProUGUI endText;

    private float _warningTimer = 0f;

    void Awake() => Instance = this;

    void Start()
    {
        if (warningText != null) warningText.text = "";
        if (endScreen != null) endScreen.SetActive(false);
    }

    void Update()
    {
        if (_warningTimer > 0f)
        {
            _warningTimer -= Time.deltaTime;
            if (_warningTimer <= 0f && warningText != null)
                warningText.text = "";
        }
    }

    public void UpdateScore(int s) { if (scoreText != null) scoreText.text = "Очки: " + s; }
    public void UpdateLives(int l) { if (livesText != null) livesText.text = "Жизни: " + l; }

    public void UpdateTimer(float normalized)
    {
        if (timerBar == null) return;
        timerBar.fillAmount = Mathf.Clamp01(normalized);
        // Краснеет, когда мало времени
        timerBar.color = Color.Lerp(Color.red, Color.green, normalized);
    }

    public void ShowWarning(string msg)
    {
        if (warningText == null) return;
        warningText.text = msg;
        _warningTimer = 3f;
    }

    public void ShowFloatingText(string msg)
    {
        ShowWarning(msg);  // MVP: используем тот же текст
    }

    public void ShowEndScreen(bool win)
    {
        if (endScreen == null) return;
        endScreen.SetActive(true);
        if (endText != null)
            endText.text = win ? "ПОБЕДА!" : "ПОРАЖЕНИЕ";
        Time.timeScale = 0f;
    }
}