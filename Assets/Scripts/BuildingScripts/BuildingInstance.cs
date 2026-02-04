using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;

    private void Start()
    {
        if (data == null) return;

        // If this building provides population, spawn townspeople
        if (data.populationProvided > 0 && TownspersonSpawner.Instance != null)
        {
            TownspersonSpawner.Instance.AddPopulation(data.populationProvided, transform.position);
        }
    }
}