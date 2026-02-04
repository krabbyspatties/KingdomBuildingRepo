using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private Dictionary<ResourceType, int> resources = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Add(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;

        Debug.Log($"{type.resourceName}: {resources[type]}");
    }

    public int Get(ResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}