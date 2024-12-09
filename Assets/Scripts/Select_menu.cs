using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public Button[] levels; // Кнопки для уровней

    private void Start()
    {
        // Загружаем прогресс для текущего слота
        int levelReached = SlotManager.GetLevelReached();

        // Обновляем доступность кнопок уровней
        for (int i = 0; i < levels.Length; i++)
        {
            if (i + 1 > levelReached)
                levels[i].interactable = false; // Блокируем уровень, если он не открыт
            else
                levels[i].interactable = true;  // Разблокируем уровень
        }
    }


    //private void Start()
    //{
    //    LoadLevelAvailability();
    //}

    //private void LoadLevelAvailability()
    //{
    //    // Получаем максимальный доступный уровень
    //    int levelReached = PlayerPrefs.GetInt("levelReached", 1);

    //    // Обновляем кнопки в зависимости от доступных уровней
    //    for (int i = 0; i < levels.Length; i++)
    //    {
    //        levels[i].interactable = (i + 1 <= levelReached);
    //    }
    //}

    public void LoadLevelN(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UnlockNextLevel(int levelIndex)
    {
        // Сохраняем прогресс, если уровень ещё не был разблокирован
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        if (levelIndex > levelReached)
        {
            PlayerPrefs.SetInt("levelReached", levelIndex);
            PlayerPrefs.Save();
        }
    }
}
