using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public void Respawn()
    {
        SaveData data = SaveSystem.LoadProgress();
        string activeCheckpointID = data.lastActiveCheckpointID;

        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.checkpointID == activeCheckpointID)
            {
                transform.position = checkpoint.GetSpawnPoint().position;
                Debug.Log("Respawned at: " + activeCheckpointID);
                return;
            }
        }

        Debug.LogWarning("No active checkpoint found!");
    }
}
