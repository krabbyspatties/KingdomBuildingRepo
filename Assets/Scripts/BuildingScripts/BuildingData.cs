using UnityEngine;

[CreateAssetMenu(menuName = "Kingdom/Building")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;

    [Header("Grid Footprint")]
    [Tooltip("Width x Height in grid cells")]
    public Vector2Int size = new Vector2Int(1, 1);

    [Header("Stats")]
    public int maxHealth = 1000;

    [Header("Housing (optional)")]
    [Tooltip("Number of townspeople this building provides")]
    public int populationProvided = 0;
}