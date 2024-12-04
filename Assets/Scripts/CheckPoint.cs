using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointID;  // Уникальный идентификатор точки сохранения
    public Transform spawnPoint; // Опциональная точка возрождения
    public bool isUnlocked;     // Разблокирован ли чекпоинт

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        SaveData data = SaveSystem.LoadProgress();

        if (!data.unlockedCheckpoints.Contains(checkpointID))
        {
            data.unlockedCheckpoints.Add(checkpointID); // Добавляем эту точку в список разблокированных
        }

        data.lastActiveCheckpointID = checkpointID; // Устанавливаем её как последнюю активную
        SaveSystem.SaveProgress(data);
        
        isUnlocked = true;

        Debug.Log("Checkpoint " + checkpointID + " activated!");
    }



    public Transform GetSpawnPoint()
    {
        // Возвращаем точку возрождения или позицию самой точки
        return spawnPoint != null ? spawnPoint : transform;
    }
}
