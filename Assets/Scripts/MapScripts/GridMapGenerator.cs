using UnityEngine;

public class GridMapGenerator : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject ground;
    public float cellSize = 1f;
    [Range(0f, 0.5f)] public float borderInset = 0.05f;

    [Header("Grid Appearance")]
    public Color lineColor = Color.green;
    public float lineWidth = 0.05f;

    [Header("Boundary Settings")]
    public bool generateBoundaries = true;
    public GameObject boundaryPrefab;
    [Header("Managers")]
    public GridManager gridManager;
    public CastleSpawner castleSpawner;
    public CaveSpawner caveSpawner;
    public TreeSpawner treeSpawner;
    public WaterBodySpawner waterSpawner;
    public TownspersonSpawner townspersonSpawner;

    private int gridWidth;
    private int gridHeight;
    private Vector2 gridOrigin;

    private static Material gridMaterial;

    void Start()
    {
        if (ground == null || gridManager == null)
        {
            Debug.LogError("GridMapGenerator: Missing references!");
            return;
        }

        Vector3 groundSize = ground.transform.localScale;
        gridWidth = Mathf.RoundToInt(groundSize.x / cellSize);
        gridHeight = Mathf.RoundToInt(groundSize.y / cellSize);

        Vector3 groundPos = ground.transform.position;
        gridOrigin = new Vector2(
            groundPos.x - groundSize.x / 2f,
            groundPos.y - groundSize.y / 2f
        );

        // Initialize the grid manager
        gridManager.Initialize(gridWidth, gridHeight, cellSize, gridOrigin);

        // Draw the grid
        GenerateGrid();

        // Spawn water first

        if (castleSpawner != null)
            castleSpawner.SpawnCastle();

        if (waterSpawner != null)
            waterSpawner.GenerateWaterBodies();

        // Spawn caves
        if (caveSpawner != null)
            caveSpawner.GenerateCaves();

        // Spawn trees
        if (treeSpawner != null)
            treeSpawner.GenerateTrees();

        // Generate boundaries
        if (generateBoundaries && boundaryPrefab != null)
            GenerateBoundaries();
    }

    // ---------- existing grid & boundary methods ----------

    void GenerateGrid()
    {
        GameObject gridParent = new GameObject("Grid");

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                DrawCellBorder(x, y, gridParent.transform);
            }
        }
    }

    void DrawCellBorder(int x, int y, Transform parent)
    {
        float inset = borderInset * cellSize;

        Vector3 bl = new(gridOrigin.x + x * cellSize + inset,
                         gridOrigin.y + y * cellSize + inset, 0);

        Vector3 br = bl + new Vector3(cellSize - inset * 2, 0, 0);
        Vector3 tl = bl + new Vector3(0, cellSize - inset * 2, 0);
        Vector3 tr = bl + new Vector3(cellSize - inset * 2, cellSize - inset * 2, 0);

        CreateLine(bl, br, parent);
        CreateLine(br, tr, parent);
        CreateLine(tr, tl, parent);
        CreateLine(tl, bl, parent);
    }

    void CreateLine(Vector3 start, Vector3 end, Transform parent)
    {
        if (gridMaterial == null)
            gridMaterial = new Material(Shader.Find("Sprites/Default"));

        GameObject line = new GameObject("Line");
        line.transform.parent = parent;

        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = gridMaterial;
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.sortingLayerName = "Ground";
    }

    void GenerateBoundaries()
    {
        GameObject parent = new GameObject("Boundaries");

        float width = gridWidth * cellSize;
        float height = gridHeight * cellSize;

        Vector3 center = new(
            gridOrigin.x + width / 2f,
            gridOrigin.y + height / 2f,
            0
        );

        CreateBoundary(
            new Vector3(center.x, gridOrigin.y + height, 0),
            new Vector3(width, cellSize, 1),
            parent.transform,
            "Top"
        );

        CreateBoundary(
            new Vector3(center.x, gridOrigin.y, 0),
            new Vector3(width, cellSize, 1),
            parent.transform,
            "Bottom"
        );

        CreateBoundary(
            new Vector3(gridOrigin.x, center.y, 0),
            new Vector3(cellSize, height, 1),
            parent.transform,
            "Left"
        );

        CreateBoundary(
            new Vector3(gridOrigin.x + width, center.y, 0),
            new Vector3(cellSize, height, 1),
            parent.transform,
            "Right"
        );
    }

    void CreateBoundary(Vector3 position, Vector3 scale, Transform parent, string name)
    {
        GameObject b = Instantiate(boundaryPrefab, position, Quaternion.identity, parent);
        b.name = name;
        b.transform.localScale = scale;
    }
}