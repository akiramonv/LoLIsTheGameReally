using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> allCheckpoints;         // ��� ������������ ���������
    public List<string> unlockedCheckpoints = new List<string>(); // ������ ID ���������������� �����
    public string lastActiveCheckpointID;  // ID ���������� ��������� ���������
}
