using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounterText;
    private static UIManager instance = null;
    public static UIManager Instance => instance;
    [SerializeField] private Character character;
    [SerializeField] private Image healthBar;
    
    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private CanvasGroup victoryCanvasGroup;
    private float fadingTime = 2f;
    private bool isFadingGameOver;
    private bool isFadingVictory;
    private class PlayerStatistics
    {
        public int coinCounter = 0;

    }

    private PlayerStatistics statistics;
    private void Awake()
    {
        instance = this;
        this.statistics = new PlayerStatistics() { coinCounter = 0 };
        this.gameOverCanvasGroup.interactable = false;
        this.gameOverCanvasGroup.blocksRaycasts = false;
        this.victoryCanvasGroup.interactable = false;
        this.victoryCanvasGroup.blocksRaycasts = false;
    }
    public void CollectCoin()
    {
        this.statistics.coinCounter++;
        string coinText = $"{this.statistics.coinCounter} ";
        this.coinCounterText.text = coinText;
    }
    
    private void Update()
    {
        float percent = this.character.GetCurrentHealth() / this.character.GetMaxHealth();
        this.healthBar.fillAmount = percent;

        if (percent <= 0.0f && !this.isFadingGameOver)
        {
            this.StartCoroutine(this.FadeInGameOver());
        }
    }

    public void Reload()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator FadeInGameOver()
    {
        this.isFadingGameOver = true;
        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer /  this.fadingTime;
            this.hudCanvasGroup.alpha = 1.0f - percent;
            this.gameOverCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.hudCanvasGroup.alpha = 0.0f;
        this.gameOverCanvasGroup.alpha = 1.0f;
        this.gameOverCanvasGroup.blocksRaycasts = true;
        this.gameOverCanvasGroup.interactable = true;
    }
    
    private IEnumerator FadeInVictory()
    {
        this.isFadingVictory = true;
        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer /  this.fadingTime;
            this.hudCanvasGroup.alpha = 1.0f - percent;
            this.victoryCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.hudCanvasGroup.alpha = 0.0f;
        this.victoryCanvasGroup.alpha = 1.0f;
        this.victoryCanvasGroup.blocksRaycasts = true;
        this.victoryCanvasGroup.interactable = true;
    }
    
    public void TriggerVictory()
    {
        this.StartCoroutine(this.FadeInVictory());
    }
}