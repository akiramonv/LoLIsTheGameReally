using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public Button[] levels; // ������ ��� �������

    private void Start()
    {
        // ��������� �������� ��� �������� �����
        int levelReached = SlotManager.GetLevelReached();

        // ��������� ����������� ������ �������
        for (int i = 0; i < levels.Length; i++)
        {
            if (i + 1 > levelReached)
                levels[i].interactable = false; // ��������� �������, ���� �� �� ������
            else
                levels[i].interactable = true;  // ������������ �������
        }
    }


    //private void Start()
    //{
    //    LoadLevelAvailability();
    //}

    //private void LoadLevelAvailability()
    //{
    //    // �������� ������������ ��������� �������
    //    int levelReached = PlayerPrefs.GetInt("levelReached", 1);

    //    // ��������� ������ � ����������� �� ��������� �������
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
        // ��������� ��������, ���� ������� ��� �� ��� �������������
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        if (levelIndex > levelReached)
        {
            PlayerPrefs.SetInt("levelReached", levelIndex);
            PlayerPrefs.Save();
        }
    }
}
