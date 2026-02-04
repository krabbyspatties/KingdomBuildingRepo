using UnityEngine;
using System.Collections.Generic;

public class WaterBodySpawner : MonoBehaviour
{
    [Header("Water Tile")]
    public WaterTileData waterTile;

    [Header("Lake Settings")]
    public int lakeCount = 4;        // 🔥 how many lakes
    public int minLakeSize = 6;
    public int maxLakeSize = 15;

    [Header("River Settings")]
    public int riverCount = 0;       // optional
    public int minRiverLength = 5;
    public int maxRiverLength = 12;

    public void GenerateWaterBodies()
    {
        // Spawn lakes
        for (int i = 0; i < lakeCount; i++)
        {
            GenerateLake();
        }

        // Spawn rivers (optional)
        for (int i = 0; i < riverCount; i++)
        {
            GenerateRiver();
        }
    }

    void GenerateLake()
    {
        int lakeSize = Random.Range(minLakeSize, maxLakeSize + 1);

        Vector2Int startCell = GridManager.Instance.GetRandomEmptyCell();
        if (startCell.x == -1) return;

        Vector2Int current = startCell;

        for (int i = 0; i < lakeSize; i++)
        {
            SpawnTile(current);
            current = GetNextCell(current);
        }
    }

    void GenerateRiver()
    {
        int length = Random.Range(minRiverLength, maxRiverLength + 1);

        Vector2Int startCell = GridManager.Instance.GetRandomEmptyCell();
        if (startCell.x == -1) return;

        Vector2Int current = startCell;

        for (int i = 0; i < length; i++)
        {
            SpawnTile(current);
            current = GetNextCell(current);
        }
    }

    void SpawnTile(Vector2Int cell)
    {
        if (!GridManager.Instance.TryOccupy(cell))
            return;

        Vector3 pos = GridManager.Instance.CellToWorld(cell);
        GameObject water = Instantiate(waterTile.prefab, pos, Quaternion.identity);

        // 🔒 Force water tile to exactly match grid cell
        GridManager.Instance.EnforceCellScale(water, Vector2Int.one);
    }

    Vector2Int GetNextCell(Vector2Int current)
    {
        List<Vector2Int> options = new List<Vector2Int>()
        {
            new Vector2Int(current.x + 1, current.y),
            new Vector2Int(current.x - 1, current.y),
            new Vector2Int(current.x, current.y + 1),
            new Vector2Int(current.x, current.y - 1)
        };

        options.RemoveAll(c =>
            !GridManager.Instance.IsInsideGrid(c) ||
            GridManager.Instance.IsOccupied(c)
        );

        if (options.Count == 0)
            return current;

        return options[Random.Range(0, options.Count)];
    }
}