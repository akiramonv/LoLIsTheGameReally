using UnityEngine;
using UnityEngine.SceneManagement; // Для управления сценами

public class LevelCompleteTrigger : MonoBehaviour
{
    [SerializeField] public GameObject levelCompletePanel; // Панель завершения уровня
    public int nextLevelIndex; // Индекс следующего уровня для разблокировки
    [SerializeField] public string NextSceneName;
    public int totalLevels = 5; // Общее количество уровней в игре
    public int slotIndex;         // Индекс текущего слота
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
            // Разблокируем следующий уровень для текущего слота
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
        // Показываем панель завершения уровня
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            Time.timeScale = 0; // Останавливаем время, чтобы игра была на паузе
        }
    }

    //public void LoadNextLevel()
    //{
    //    Time.timeScale = 1; // Возобновляем время перед загрузкой
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Загружаем следующий уровень
    //}

    public void LoadNextLevel()
    {
        Time.timeScale = 1; // Возобновляем время перед загрузкой
        if (!string.IsNullOrEmpty(NextSceneName))
        {
            SceneManager.LoadScene(NextSceneName); // Загружаем сцену по названию
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }

    private void UnlockNextLevel(int levelIndex)
    {
        // Сохраняем прогресс в PlayerPrefs
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
            PlayerPrefs.SetInt(levelKey, levelIndex); // Сохраняем прогресс уровня для данного слота
        }
    }

    private void UpdateCompletionPercentage()
    {
        string levelKey = $"Slot{slotIndex}_levelReached";
        string completionKey = $"Slot{slotIndex}_Completion";

        int unlockedLevels = PlayerPrefs.GetInt(levelKey, 1); // Количество открытых уровней
        float completion = (float)unlockedLevels / totalLevels * 100f; // Рассчитываем процент
        PlayerPrefs.SetFloat(completionKey, completion); // Сохраняем процент завершения для данного слота
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex}: Completion updated to {completion}%");
    }
}
