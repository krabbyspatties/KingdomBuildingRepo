using System.Collections.Generic;
using UnityEngine;

public class TownspersonSpawner : MonoBehaviour
{
    public static TownspersonSpawner Instance;

    public TownspersonData townspersonData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // only one spawner allowed
    }

    public void AddPopulation(int amount, Vector3 origin)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2Int cell = GetRandomCellNear(origin);
            if (cell.x == -1) continue;

            Vector3 pos = GridManager.Instance.CellToWorld(cell);
            GameObject person = Instantiate(townspersonData.prefab, pos, Quaternion.identity);

            GridManager.Instance.TryOccupy(cell);
            GridManager.Instance.EnforceCellScale(person);

            Townsperson tp = person.GetComponent<Townsperson>();
            if (tp != null)
                tp.data = townspersonData;
        }
    }

    Vector2Int GetRandomCellNear(Vector3 worldPos, int radius = 3)
    {
        Vector2Int center = GridManager.Instance.WorldToCell(worldPos);
        List<Vector2Int> candidates = new();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector2Int cell = center + new Vector2Int(x, y);
                if (!GridManager.Instance.IsInsideGrid(cell)) continue;
                if (GridManager.Instance.IsOccupied(cell)) continue;
                candidates.Add(cell);
            }
        }

        return candidates.Count == 0 ? new Vector2Int(-1, -1) : candidates[Random.Range(0, candidates.Count)];
    }
}