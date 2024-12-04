using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> allCheckpoints;         // Все существующие чекпоинты
    public List<string> unlockedCheckpoints = new List<string>(); // Список ID разблокированных точек
    public string lastActiveCheckpointID;  // ID последнего активного чекпоинта
}
