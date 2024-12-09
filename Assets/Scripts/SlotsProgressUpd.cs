using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private Text progressText; // Текст для отображения прогресса
    [SerializeField] private int slotIndex;     // Индекс данного слота

    public void LoadSlot()
    {
        SlotManager.currentSlotIndex = slotIndex; // Устанавливаем текущий слот
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // Сохраняем выбранный слот
        PlayerPrefs.Save();
        Debug.Log($"Slot {slotIndex} loaded.");

        // Загрузка сцены с выбором уровней или начальной сцены
        UnityEngine.SceneManagement.SceneManager.LoadScene("Select_level_menu");
    }

    private void Start()
    {
        string completionKey = $"Slot{slotIndex}_Completion";
        float completionPercentage = PlayerPrefs.GetFloat(completionKey, 0f);
        progressText.text = $"Progress: {completionPercentage:F1}%";
    }
}
