using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public BuildingData buildingToPlace;

    private bool isPlacing;
    private GameObject previewInstance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isPlacing)
            {
                // If already placing, cancel instead of starting a new one
                CancelPlacing();
            }
            else
            {
                StartPlacing();
            }
        }

        if (!isPlacing)
            return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceBuilding();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacing();
        }
    }



    void StartPlacing()
    {
        if (buildingToPlace == null)
        {
            Debug.LogWarning("No BuildingData assigned!");
            return;
        }

        isPlacing = true;
        previewInstance = Instantiate(buildingToPlace.prefab);
        SetPreviewVisual(previewInstance);
    }

    void CancelPlacing()
    {
        isPlacing = false;

        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null; // clear reference to avoid leftover highlights
        }
    }

    void UpdatePreviewPosition()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2Int cell = GridManager.Instance.WorldToCell(mouseWorld);
        Vector2Int originCell = GetFootprintOrigin(cell);

        List<Vector2Int> footprint = GetFootprintCells(originCell);
        Vector3 center = GetFootprintCenter(footprint);

        previewInstance.transform.position = center;

        GridManager.Instance.EnforceCellScale(previewInstance, buildingToPlace.size);

        bool valid = GridManager.Instance.CanOccupy(footprint);
        SetPreviewColor(valid ? Color.green : Color.red);
    }

    void TryPlaceBuilding()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2Int cell = GridManager.Instance.WorldToCell(mouseWorld);
        Vector2Int originCell = GetFootprintOrigin(cell);

        List<Vector2Int> footprint = GetFootprintCells(originCell);

        if (!GridManager.Instance.CanOccupy(footprint))
            return;

        GridManager.Instance.OccupyCells(footprint);

        Vector3 center = GetFootprintCenter(footprint);
        GameObject building = Instantiate(buildingToPlace.prefab, center, Quaternion.identity);

        GridManager.Instance.EnforceCellScale(building, buildingToPlace.size);

        // Initialize health if it’s a castle (optional)
        Castle castle = building.GetComponent<Castle>();
        if (castle != null)
            castle.Initialize(buildingToPlace);

        // --- NEW: spawn population if this building provides it ---
        if (buildingToPlace.populationProvided > 0)
        {
            TownspersonSpawner.Instance.AddPopulation(buildingToPlace.populationProvided, building.transform.position);
        }

        CancelPlacing();
    }

    // -------- helpers --------

    Vector2Int GetFootprintOrigin(Vector2Int cell)
    {
        return new Vector2Int(
            cell.x - buildingToPlace.size.x / 2,
            cell.y - buildingToPlace.size.y / 2
        );
    }

    List<Vector2Int> GetFootprintCells(Vector2Int origin)
    {
        List<Vector2Int> cells = new();
        for (int x = 0; x < buildingToPlace.size.x; x++)
            for (int y = 0; y < buildingToPlace.size.y; y++)
                cells.Add(new Vector2Int(origin.x + x, origin.y + y));
        return cells;
    }

    Vector3 GetFootprintCenter(List<Vector2Int> cells)
    {
        Vector3 sum = Vector3.zero;
        foreach (var c in cells)
            sum += GridManager.Instance.CellToWorld(c);
        return sum / cells.Count;
    }

    void SetPreviewVisual(GameObject go)
    {
        foreach (var sr in go.GetComponentsInChildren<SpriteRenderer>())
            sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    void SetPreviewColor(Color c)
    {
        foreach (var sr in previewInstance.GetComponentsInChildren<SpriteRenderer>())
            sr.color = new Color(c.r, c.g, c.b, 0.5f);
    }
}