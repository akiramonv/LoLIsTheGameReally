using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton ��� ������� �� ������ �������
    public Text scoreTextGame;              // ������ �� ����� ��� ����������� �����
    public Text scoreTextFinal;              // ������ �� ����� ��� ����������� �����
    public int score = 0;               // ������� ���������� �����
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
        UpdateScoreText(); // ��������� ����� � ������ ����
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
        scoreTextGame.text = "Score: " + score; // ��������� �����
        scoreTextFinal.text = "Score: " + score; // ��������� �����

    }
}
