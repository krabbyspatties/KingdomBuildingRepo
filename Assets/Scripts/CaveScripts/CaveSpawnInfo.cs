using UnityEngine;

[System.Serializable]
public struct CaveSpawnInfo
{
    public CaveData data;

    [Tooltip("Guaranteed minimum caves to spawn (will be at least 1)")]
    public int minAmount;
}