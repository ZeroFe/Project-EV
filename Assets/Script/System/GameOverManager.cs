using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerStatus baseStatus;

    [SerializeField] private float gameOverFadeOutTime = 2.0f;

    [Header("UI")]
    private CanvasGroup group;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverReasonText;
    [SerializeField, TextArea(3, 5)] 
    private string reasonPlayerDeath;
    [SerializeField, TextArea(3, 5)]
    private string reasonBaseDestroyed;
    [SerializeField]
    private Button restartButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip gameOverBGM;

    private void Awake()
    {
        Instance = this;

        Debug.Assert(gameOverFadeOutTime > 0.0f, "Error : game over fade out time must larger than 0");

        group = gameOverPanel.GetComponent<CanvasGroup>();
        Debug.Assert(playerStatus, "Error : " + nameof(playerStatus) + " is not set");
        Debug.Assert(baseStatus, "Error : " + nameof(baseStatus) + " is not set");
        Debug.Assert(gameOverReasonText, "Error : " + nameof(gameOverReasonText) + " is not set");
    }

    private void Start()
    {
        playerStatus.OnDead += () => GameOver(true);
        baseStatus.OnDead += () => GameOver(false);
    }

    public void GameOver(bool gameoverByPlayerDeath)
    {
        StartCoroutine(IEGameOver(gameoverByPlayerDeath));
    }

    IEnumerator IEGameOver(bool gameoverByPlayerDeath)
    {
        restartButton.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        group.alpha = 0.0f;
        gameOverReasonText.text = "";
        AudioManager.Instance.PlayBGM(AudioManager.BgmType.GameOver);

        yield return IEFadeOut();

        yield return IETextDraw(gameoverByPlayerDeath);

        WindowSystem.Instance.OpenWindow(restartButton.gameObject, false);
    }

    IEnumerator IEFadeOut()
    {
        float interval = 0.05f;
        for (float t = 0; t <= gameOverFadeOutTime; t += interval)
        {
            float percent = Mathf.Clamp01(t / gameOverFadeOutTime);;
            Time.timeScale = 1 - percent;
            group.alpha = percent;
            yield return new WaitForSecondsRealtime(interval);
        }

        Time.timeScale = 0.0f;
        group.alpha = 1.0f;
    }

    IEnumerator IETextDraw(bool gameoverByPlayerDeath)
    {
        float interval = 0.1f;
        string reasonStr = gameoverByPlayerDeath ? reasonPlayerDeath : reasonBaseDestroyed;

        StringBuilder sb = new StringBuilder(reasonStr.Length);
        for (int idx = 0; sb.Length < reasonStr.Length; idx++)
        {
            sb.Append(reasonStr[idx]);
            gameOverReasonText.text = sb.ToString();
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
