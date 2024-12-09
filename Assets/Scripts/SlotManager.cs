using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static int currentSlotIndex; // “екущий выбранный слот

    public void SelectSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex; // ”станавливаем активный слот
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // —охран€ем активный слот
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex} selected.");
    }

    private void Start()
    {
        // ≈сли слот не был установлен ранее, используем первый
        currentSlotIndex = PlayerPrefs.GetInt("CurrentSlot", 1);
    }

    // ћетод дл€ получени€ прогресса дл€ текущего слота
    public static int GetLevelReached()
    {
        return PlayerPrefs.GetInt($"Slot{currentSlotIndex}_LevelReached", 1);
    }

    // ћетод дл€ сохранени€ прогресса дл€ текущего слота
    public static void SaveProgress(int levelReached)
    {
        PlayerPrefs.SetInt($"Slot{currentSlotIndex}_LevelReached", levelReached);
        PlayerPrefs.Save();
        Debug.Log($"Progress saved for Slot {currentSlotIndex}: Level {levelReached}");
    }
}
