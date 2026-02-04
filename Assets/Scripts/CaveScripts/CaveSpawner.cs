using UnityEngine;

public class CaveSpawner : MonoBehaviour
{
    [Header("Cave Prefab")]
    public GameObject cavePrefab;

    [Header("Cave Types")]
    public CaveSpawnInfo[] caveTypes;

    public void GenerateCaves()
    {
        foreach (var caveType in caveTypes)
        {
            int min = Mathf.Max(1, caveType.minAmount);
            int max = min + 2;
            int spawnCount = Random.Range(min, max + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                if (i >= min && Random.value > caveType.data.rarity)
                    continue;

                SpawnCave(caveType.data);
            }
        }
    }

    void SpawnCave(CaveData data)
    {
        Vector2Int cell = GridManager.Instance.GetRandomEmptyCell();
        if (cell.x == -1)
            return;

        if (!GridManager.Instance.TryOccupy(cell))
            return;

        Vector3 position = GridManager.Instance.CellToWorld(cell);

        GameObject cave = Instantiate(cavePrefab, position, Quaternion.identity);
        CaveInstance instance = cave.GetComponent<CaveInstance>();
        if (instance != null)
            instance.data = data;
    }
}