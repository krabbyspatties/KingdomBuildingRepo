using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kingdom/Cave")]
public class CaveData : ScriptableObject
{
    public string caveName;

    [Header("Production")]
    public float tickInterval = 1f;
    public Color displayColor = Color.white;

    [System.Serializable]
    public class ResourceOutput
    {
        public ResourceType resource;
        public int amountPerTick;
    }

    public List<ResourceOutput> outputs;

    [Header("Rarity Settings")]
    [Tooltip("Probability weight or spawn rarity. Higher = more common")]
    [Range(0f, 1f)]
    public float rarity = 1f;
}