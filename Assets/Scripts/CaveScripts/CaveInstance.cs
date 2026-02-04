using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CaveInstance : MonoBehaviour
{
    public CaveData data;
    private SpriteRenderer spriteRenderer;

    private Coroutine productionRoutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (data != null)
            spriteRenderer.color = data.displayColor;
        else
            Debug.LogWarning($"CaveInstance on {gameObject.name} has no CaveData assigned.");

        // ❌ REMOVE automatic production
        // StartCoroutine(ProduceResources());
    }

    // ✅ Call this later when a worker is assigned
    public void StartProduction()
    {
        if (productionRoutine == null && data != null)
            productionRoutine = StartCoroutine(ProduceResources());
    }

    // ✅ Call this when worker leaves / dies
    public void StopProduction()
    {
        if (productionRoutine != null)
        {
            StopCoroutine(productionRoutine);
            productionRoutine = null;
        }
    }

    private IEnumerator ProduceResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(data.tickInterval);

            foreach (var output in data.outputs)
            {
                ResourceManager.Instance.Add(output.resource, output.amountPerTick);
            }
        }
    }
}