using System.Collections.Generic;
using UnityEngine;

public class CastleSpawner : MonoBehaviour
{
    public static CastleSpawner Instance;

    public BuildingData castleData;

    [HideInInspector] public Vector2Int castleStartCell;
    [HideInInspector] public Vector2Int castleSize;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCastle()
    {
        Vector2Int startCell = GetCenterCell();
        List<Vector2Int> footprint = GetFootprintCells(startCell, castleData.size);

        // Occupy cells
        foreach (var cell in footprint)
            GridManager.Instance.TryOccupy(cell);

        // Save footprint info
        castleStartCell = startCell;
        castleSize = castleData.size;

        // Spawn prefab at center
        Vector3 worldPos = GetFootprintCenter(footprint);
        GameObject castleGO = Instantiate(castleData.prefab, worldPos, Quaternion.identity);

        // Scale to grid footprint
        GridManager.Instance.EnforceCellScale(castleGO, castleData.size);

        // Init castle health
        Castle castle = castleGO.GetComponent<Castle>();
        if (castle == null)
            castle = castleGO.AddComponent<Castle>();

        castle.Initialize(castleData);
    }

    List<Vector2Int> GetFootprintCells(Vector2Int startCell, Vector2Int size)
    {
        List<Vector2Int> cells = new();
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                cells.Add(new Vector2Int(startCell.x + x, startCell.y + y));
        return cells;
    }

    Vector2Int GetCenterCell()
    {
        int x = GridManager.Instance.Width / 2 - castleData.size.x / 2;
        int y = GridManager.Instance.Height / 2 - castleData.size.y / 2;
        return new Vector2Int(x, y);
    }

    Vector3 GetFootprintCenter(List<Vector2Int> cells)
    {
        Vector3 sum = Vector3.zero;
        foreach (var c in cells)
            sum += GridManager.Instance.CellToWorld(c);
        return sum / cells.Count;
    }
}