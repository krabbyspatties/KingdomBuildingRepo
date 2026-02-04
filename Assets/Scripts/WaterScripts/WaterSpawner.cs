using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public WaterTileData[] waterTypes;
    public int tilesPerType = 10;

    public void GenerateWater()
    {
        foreach (var waterType in waterTypes)
        {
            for (int i = 0; i < tilesPerType; i++)
            {
                SpawnWater(waterType);
            }
        }
    }

    void SpawnWater(WaterTileData data)
    {
        Vector2Int cell = GridManager.Instance.GetRandomEmptyCell();
        if (cell.x == -1) return;
        if (!GridManager.Instance.TryOccupy(cell)) return;

        Vector3 pos = GridManager.Instance.CellToWorld(cell);
        GameObject water = Instantiate(data.prefab, pos, Quaternion.identity);

        // 🔒 Enforce prefab to exactly match grid cell size
        GridManager.Instance.EnforceCellScale(water);
    }
}