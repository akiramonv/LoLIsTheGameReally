using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject slotsPanel; // ������ �� �������
    [SerializeField] private Button[] slotButtons;  // ������ ��� ���� ������

    private int selectedSlotIndex = -1; // ������ ���������� �����

    

    public void OpenSaveSlots()
    {
        slotsPanel.SetActive(true);
        LoadSlots(); // �������� ��������� �� ����������

    }

    public void SelectSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        Debug.Log($"Selected Slot {slotIndex + 1} with progress: {PlayerPrefs.GetInt($"Slot_{slotIndex}_Progress", 0)}");
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // ��������� ��������� ����
        SceneManager.LoadScene(1); // ��������� ������ �������
    }

    private void LoadSlots()
    {
        // ��������� ������ ��� ������� �����
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int progress = PlayerPrefs.GetInt($"Slot_{i}_Progress", 0);
            slotButtons[i].GetComponentInChildren<Text>().text = $"Slot {i + 1} - Progress: {progress}%";

            // ��������� ���������� ������� ��� ������ ������
            int index = i; // ��������� ����� ���������� ��� ���������
            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => SelectSlot(index));
        }
    }

    public void SaveProgress(int slotIndex, int progress)
    {
        PlayerPrefs.SetInt($"Slot_{slotIndex}_Progress", progress);
        PlayerPrefs.Save();
    }
}
