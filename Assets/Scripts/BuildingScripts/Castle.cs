using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    // Call this to initialize health from BuildingData
    public void Initialize(BuildingData data)
    {
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Castle destroyed!");
        }
    }
}