using UnityEngine;

[CreateAssetMenu(menuName = "Kingdom/WaterTile")]
public class WaterTileData : ScriptableObject
{
    public string tileName;
    public GameObject prefab; // MUST have SpriteRenderer sized to 1 grid cell
}