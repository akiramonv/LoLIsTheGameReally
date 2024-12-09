using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static int currentSlotIndex; // ������� ��������� ����

    public void SelectSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex; // ������������� �������� ����
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // ��������� �������� ����
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex} selected.");
    }

    private void Start()
    {
        // ���� ���� �� ��� ���������� �����, ���������� ������
        currentSlotIndex = PlayerPrefs.GetInt("CurrentSlot", 1);
    }

    // ����� ��� ��������� ��������� ��� �������� �����
    public static int GetLevelReached()
    {
        return PlayerPrefs.GetInt($"Slot{currentSlotIndex}_LevelReached", 1);
    }

    // ����� ��� ���������� ��������� ��� �������� �����
    public static void SaveProgress(int levelReached)
    {
        PlayerPrefs.SetInt($"Slot{currentSlotIndex}_LevelReached", levelReached);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved for Slot {currentSlotIndex}: Level {levelReached}");
    }
}
