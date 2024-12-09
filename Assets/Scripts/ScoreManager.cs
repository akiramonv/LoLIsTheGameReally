using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton для доступа из других классов
    public Text scoreTextGame;              // Ссылка на текст для отображения очков
    public Text scoreTextFinal;              // Ссылка на текст для отображения очков
    public int score = 0;               // Текущее количество очков
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText(); // Обновляем текст в начале игры
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        scoreTextGame.text = "Score: " + score; // Обновляем текст
        scoreTextFinal.text = "Score: " + score; // Обновляем текст

    }
}
