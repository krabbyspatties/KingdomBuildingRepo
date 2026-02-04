using UnityEngine;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    [Header("Tree Prefab")]
    public GameObject treePrefab;

    [Header("Tree Types")]
    public TreeSpawnInfo[] treeTypes;

    [Header("Randomization")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float maxRotation = 30f;

    [Header("Forest Settings")]
    [Range(0f, 1f)]
    public float forestChance = 0.3f;      // Chance a tree spawns a forest
    public int minForestSize = 3;
    public int maxForestSize = 10;

    [Range(0f, 1f)]
    public float forestDensity = 0.7f;     // 0 = spread out, 1 = compact

    public void GenerateTrees()
    {
        foreach (var treeType in treeTypes)
        {
            int min = Mathf.Max(1, treeType.minAmount);
            int max = min + 2;
            int spawnCount = Random.Range(min, max + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                if (Random.value < forestChance)
                    SpawnForest(treeType.data);
                else
                    SpawnTree(treeType.data);
            }
        }
    }

    void SpawnForest(TreeData data)
    {
        int forestSize = Random.Range(minForestSize, maxForestSize + 1);
        List<Vector2Int> forestCells = new List<Vector2Int>();

        // Pick the first empty cell
        Vector2Int startCell = GridManager.Instance.GetRandomEmptyCell();
        if (startCell.x == -1 || !GridManager.Instance.TryOccupy(startCell))
            return;

        SpawnTreeAtCell(data, startCell);
        forestCells.Add(startCell);

        for (int i = 1; i < forestSize; i++)
        {
            List<Vector2Int> potentialCells = new List<Vector2Int>();

            // Collect neighbors of all current forest cells
            foreach (var cell in forestCells)
            {
                foreach (var n in GetNeighborCells(cell))
                {
                    if (GridManager.Instance.IsInsideGrid(n) && !GridManager.Instance.IsOccupied(n) && !potentialCells.Contains(n))
                    {
                        // Add neighbor based on density
                        if (Random.value < forestDensity)
                            potentialCells.Add(n);
                    }
                }
            }

            if (potentialCells.Count == 0)
                break;

            Vector2Int chosen = potentialCells[Random.Range(0, potentialCells.Count)];
            if (GridManager.Instance.TryOccupy(chosen))
            {
                SpawnTreeAtCell(data, chosen);
                forestCells.Add(chosen);
            }
        }
    }

    void SpawnTree(TreeData data)
    {
        Vector2Int cell = GridManager.Instance.GetRandomEmptyCell();
        if (cell.x == -1 || !GridManager.Instance.TryOccupy(cell))
            return;

        SpawnTreeAtCell(data, cell);
    }

    void SpawnTreeAtCell(TreeData data, Vector2Int cell)
    {
        Vector3 pos = GridManager.Instance.CellToWorld(cell);
        GameObject tree = Instantiate(treePrefab, pos, Quaternion.identity);

        // Color
        SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
        if (sr != null && data != null)
            sr.color = data.displayColor;

        // Scale
        float scale = Random.Range(minScale, maxScale);
        tree.transform.localScale = new Vector3(scale, scale, 1f);

        // Rotation
        float rotZ = Random.Range(-maxRotation, maxRotation);
        tree.transform.Rotate(0, 0, rotZ);
    }

    Vector2Int[] GetNeighborCells(Vector2Int origin)
    {
        return new Vector2Int[]
        {
            new(origin.x + 1, origin.y),
            new(origin.x - 1, origin.y),
            new(origin.x, origin.y + 1),
            new(origin.x, origin.y - 1),
            new(origin.x + 1, origin.y + 1),
            new(origin.x - 1, origin.y + 1),
            new(origin.x + 1, origin.y - 1),
            new(origin.x - 1, origin.y - 1)
        };
    }
}