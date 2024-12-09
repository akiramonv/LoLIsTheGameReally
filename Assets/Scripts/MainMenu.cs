using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject slotsPanel; // Панель со слотами
    [SerializeField] private Button[] slotButtons;  // Кнопки для трех слотов

    private int selectedSlotIndex = -1; // Индекс выбранного слота

    

    public void OpenSaveSlots()
    {
        slotsPanel.SetActive(true);
        LoadSlots(); // Загрузка прогресса из сохранений

    }

    public void SelectSlot(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        Debug.Log($"Selected Slot {slotIndex + 1} with progress: {PlayerPrefs.GetInt($"Slot_{slotIndex}_Progress", 0)}");
        PlayerPrefs.SetInt("CurrentSlot", slotIndex); // Сохраняем выбранный слот
        SceneManager.LoadScene(1); // Загружаем первый уровень
    }

    private void LoadSlots()
    {
        // Загружаем данные для каждого слота
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int progress = PlayerPrefs.GetInt($"Slot_{i}_Progress", 0);
            slotButtons[i].GetComponentInChildren<Text>().text = $"Slot {i + 1} - Progress: {progress}%";

            // Добавляем обработчик нажатия для каждой кнопки
            int index = i; // Локальная копия переменной для замыкания
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
