using UnityEngine;

[System.Serializable]
public struct TreeSpawnInfo
{
    public TreeData data;
    [Tooltip("Minimum trees to spawn")]
    public int minAmount;
}