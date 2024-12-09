using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Text progressText; // ����� ��� ����������� ���������
    [SerializeField] private int slotIndex;     // ������ ������� �����

    public void LoadSlot()
    {
        SlotManager.currentSlotIndex = slotIndex; // ������������� ������� ����
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // ��������� ��������� ����
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex} loaded.");

        // �������� ����� � ������� ������� ��� ��������� �����
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select_level_menu");
    }

    private void Start()
    {
        string completionKey = $"Slot{slotIndex}_Completion";
        float completionPercentage = PlayerPrefs.GetFloat(completionKey, 0f);
        progressText.text = $"Progress: {completionPercentage:F1}%";
    }
}
