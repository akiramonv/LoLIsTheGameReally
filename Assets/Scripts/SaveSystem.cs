using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/saveData.json"; // ���� ��� ���������� �����

    // ��������� ��������
    public static void SaveProgress(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // ����������� ������ � JSON
        File.WriteAllText(savePath, json); // ��������� ����
        Debug.Log("Game saved: " + savePath);
    }

    // ��������� ��������
    public static SaveData LoadProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json); // ������������� ������ �� JSON
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new save data.");
            return new SaveData(); // ���� ���� �� ������, ���������� ������ ������
        }
    }
}

