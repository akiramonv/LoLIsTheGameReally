using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/saveData.json"; // Путь для сохранения файла

    // Сохранить прогресс
    public static void SaveProgress(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // Преобразуем данные в JSON
        File.WriteAllText(savePath, json); // Сохраняем файл
        Debug.Log("Game saved: " + savePath);
    }

    // Загрузить прогресс
    public static SaveData LoadProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json); // Десериализуем данные из JSON
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new save data.");
            return new SaveData(); // Если файл не найден, возвращаем пустые данные
        }
    }
}

