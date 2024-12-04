using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointID;  // ���������� ������������� ����� ����������
    public Transform spawnPoint; // ������������ ����� �����������
    public bool isUnlocked;     // ������������� �� ��������

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
            data.unlockedCheckpoints.Add(checkpointID); // ��������� ��� ����� � ������ ����������������
        }

        data.lastActiveCheckpointID = checkpointID; // ������������� � ��� ��������� ��������
        SaveSystem.SaveProgress(data);
        
        isUnlocked = true;

        Debug.Log("Checkpoint " + checkpointID + " activated!");
    }



    public Transform GetSpawnPoint()
    {
        // ���������� ����� ����������� ��� ������� ����� �����
        return spawnPoint != null ? spawnPoint : transform;
    }
}
