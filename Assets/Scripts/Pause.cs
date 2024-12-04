using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;          // ������ �����
    [SerializeField] private GameObject CheckPointPanel;          // ������ �����

    private bool isPaused = false;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        // ��������� ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) PauseOff();
            else SetPause();
        }
    }

    public void SetPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void PauseOff()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void SelectCP()
    {
        CheckPointPanel.SetActive(true);
        pausePanel.SetActive(false);
        CheckpointMenu checkpointManager = FindObjectOfType<CheckpointMenu>();
        checkpointManager.LoadCheckpointButtons();

    }
  
}
