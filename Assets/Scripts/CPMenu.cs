using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CheckpointMenu : MonoBehaviour
{
    public GameObject buttonPrefab;          // ������ ������
    public Transform buttonContainer;        // ��������� ��� ������
    public List<Checkpoint> allCheckpoints;  // ������ ���� ���������� �� �����
    [SerializeField] private GameObject CheckPointPanel; // ������ ����������
    [SerializeField] private GameObject pausePanel;      // ������ �����
    [SerializeField] private GameObject DeathPanel;      // ������ ������

    private void Start()
    {
        LoadCheckpointData(); // ��������� ���������������� ���������
        LoadCheckpointButtons(); // ������� ������
    }

    private void Awake()
    {
        CheckPointPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CheckPointPanel.SetActive(false);
        }
    }

    public void LoadCheckpointButtons()
    {
        // ������� ������ ������ ����� ����������� �����
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject); // ������� ������ ������
        }

        if (allCheckpoints == null || allCheckpoints.Count == 0)
        {
            Debug.LogError("No checkpoints available!");
            return;
        }

        // ������� ������ ��� ������� ���������
        foreach (Checkpoint checkpoint in allCheckpoints)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            Text buttonText = button.GetComponentInChildren<Text>();

            if (buttonText != null)
            {
                buttonText.text = checkpoint.checkpointID;
            }
            else
            {
                Debug.LogError("Button prefab is missing a Text component!");
                return;
            }

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.interactable = checkpoint.isUnlocked;

            if (checkpoint.isUnlocked)
            {
                buttonComponent.onClick.AddListener(() => SelectCheckpoint(checkpoint));
            }
            else
            {
                Color color = button.GetComponent<Image>().color;
                color.a = 0.5f;
                button.GetComponent<Image>().color = color;
            }
        }
    }

    public void UnlockCheckpoint(string checkpointID)
    {
        Checkpoint checkpoint = allCheckpoints.Find(c => c.checkpointID == checkpointID);

        if (checkpoint != null)
        {
            checkpoint.isUnlocked = true;
            SaveCheckpointData(); // ��������� ��������� ����������
            LoadCheckpointButtons();
        }
        else
        {
            Debug.LogError($"Checkpoint with ID {checkpointID} not found!");
        }
    }

    public void SelectCheckpoint(Checkpoint checkpoint)
    {
        Debug.Log("Checkpoint selected: " + checkpoint.checkpointID);
        RespawnPlayer(checkpoint);
    }

    public void RespawnPlayer(Checkpoint checkpoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = checkpoint.spawnPoint.position;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    private void SaveCheckpointData()
    {
        List<string> unlockedCheckpoints = new List<string>();

        foreach (Checkpoint checkpoint in allCheckpoints)
        {
            if (checkpoint.isUnlocked)
            {
                unlockedCheckpoints.Add(checkpoint.checkpointID);
            }
        }

        SaveData data = SaveSystem.LoadProgress();
        data.unlockedCheckpoints = unlockedCheckpoints;
        SaveSystem.SaveProgress(data);
    }

    private void LoadCheckpointData()
    {
        SaveData data = SaveSystem.LoadProgress();

        if (data != null && data.unlockedCheckpoints != null)
        {
            foreach (Checkpoint checkpoint in allCheckpoints)
            {
                checkpoint.isUnlocked = data.unlockedCheckpoints.Contains(checkpoint.checkpointID);
            }
        }
    }
}
