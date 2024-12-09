using UnityEngine;
using UnityEngine.SceneManagement; // ��� ���������� �������

public class LevelCompleteTrigger : MonoBehaviour
{
    [SerializeField] public GameObject levelCompletePanel; // ������ ���������� ������
    public int nextLevelIndex; // ������ ���������� ������ ��� �������������
    [SerializeField] public string NextSceneName;
    public int totalLevels = 5; // ����� ���������� ������� � ����
    public int slotIndex;         // ������ �������� �����
    private void Awake()
    {
        levelCompletePanel.SetActive(false);
    }
    private void Start()
    {
        levelCompletePanel.SetActive(false);

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            // ������������ ��������� ������� ��� �������� �����
            int levelReached = SlotManager.GetLevelReached();
            if (nextLevelIndex > levelReached)
            {
                SlotManager.SaveProgress(nextLevelIndex);
                Debug.Log($"Level {nextLevelIndex} unlocked!");
            }
            ShowLevelComplete();
            UnlockNextLevel(nextLevelIndex);
            IncrementProgress(nextLevelIndex);
            UpdateCompletionPercentage();

        }
    }

    private void ShowLevelComplete()
    {
        // ���������� ������ ���������� ������
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            Time.timeScale = 0; // ������������� �����, ����� ���� ���� �� �����
        }
    }

    //public void LoadNextLevel()
    //{
    //    Time.timeScale = 1; // ������������ ����� ����� ���������
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // ��������� ��������� �������
    //}

    public void LoadNextLevel()
    {
        Time.timeScale = 1; // ������������ ����� ����� ���������
        if (!string.IsNullOrEmpty(NextSceneName))
        {
            SceneManager.LoadScene(NextSceneName); // ��������� ����� �� ��������
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }

    private void UnlockNextLevel(int levelIndex)
    {
        // ��������� �������� � PlayerPrefs
        int currentMaxLevel = PlayerPrefs.GetInt("levelReached", 1);

        if (levelIndex > currentMaxLevel)
        {
            PlayerPrefs.SetInt("levelReached", levelIndex);
            PlayerPrefs.Save();
        }
    }

    private void IncrementProgress(int levelIndex)
    {
        string levelKey = $"Slot{slotIndex}_levelReached";
        int currentMaxLevel = PlayerPrefs.GetInt(levelKey, 1);

        if (levelIndex > currentMaxLevel)
        {
            PlayerPrefs.SetInt(levelKey, levelIndex); // ��������� �������� ������ ��� ������� �����
        }
    }

    private void UpdateCompletionPercentage()
    {
        string levelKey = $"Slot{slotIndex}_levelReached";
        string completionKey = $"Slot{slotIndex}_Completion";

        int unlockedLevels = PlayerPrefs.GetInt(levelKey, 1); // ���������� �������� �������
        float completion = (float)unlockedLevels / totalLevels * 100f; // ������������ �������
        PlayerPrefs.SetFloat(completionKey, completion); // ��������� ������� ���������� ��� ������� �����
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex}: Completion updated to {completion}%");
    }
}
