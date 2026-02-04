using UnityEngine;

[CreateAssetMenu(menuName = "Kingdom/Townsperson")]
public class TownspersonData : ScriptableObject
{
    public string personName;

    [Header("Prefab")]
    [Tooltip("Prefab used to spawn this townsperson")]
    public GameObject prefab;

    [Header("Stats")]
    public int maxHealth = 100;
    public float workSpeed = 1f;

    [Header("Visual")]
    public Sprite sprite;
}