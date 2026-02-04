using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }
    public Vector2 Origin { get; private set; }

    private HashSet<Vector2Int> occupiedCells = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Initialize(int width, int height, float cellSize, Vector2 origin)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        Origin = origin;
    }

    // -------- Grid math --------

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - Origin.x) / CellSize);
        int y = Mathf.FloorToInt((worldPos.y - Origin.y) / CellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            Origin.x + (cell.x + 0.5f) * CellSize,
            Origin.y + (cell.y + 0.5f) * CellSize,
            0
        );
    }

    // -------- Validation --------

    public bool IsInsideGrid(Vector2Int cell)
    {
        return cell.x >= 0 && cell.y >= 0 &&
               cell.x < Width && cell.y < Height;
    }

    public bool IsOccupied(Vector2Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    public bool TryOccupy(Vector2Int cell)
    {
        if (!IsInsideGrid(cell) || IsOccupied(cell))
            return false;

        occupiedCells.Add(cell);
        return true;
    }

    public void Free(Vector2Int cell)
    {
        occupiedCells.Remove(cell);
    }

    public Vector2Int GetRandomEmptyCell()
    {
        List<Vector2Int> empty = new();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2Int cell = new(x, y);
                if (!occupiedCells.Contains(cell))
                    empty.Add(cell);
            }
        }

        return empty.Count == 0
            ? new Vector2Int(-1, -1)
            : empty[Random.Range(0, empty.Count)];
    }
    public void EnforceCellScale(GameObject obj, Vector2Int footprintSize = default)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
            return;

        Vector2 spriteSize = sr.sprite.bounds.size;

        // Default to 1x1 if not provided
        if (footprintSize == Vector2Int.zero)
            footprintSize = Vector2Int.one;

        float scaleX = (CellSize * footprintSize.x) / spriteSize.x;
        float scaleY = (CellSize * footprintSize.y) / spriteSize.y;

        obj.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
    public bool CanOccupy(List<Vector2Int> cells)
    {
        foreach (var cell in cells)
        {
            if (!IsInsideGrid(cell) || IsOccupied(cell))
                return false;
        }
        return true;
    }

    public void OccupyCells(List<Vector2Int> cells)
    {
        foreach (var cell in cells)
            occupiedCells.Add(cell);
    }
}