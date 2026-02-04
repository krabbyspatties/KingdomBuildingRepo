using UnityEngine;
using System.Collections.Generic;

public class ResourceUI : MonoBehaviour
{
    public GameObject slotPrefab; // Prefab with ResourceSlotUI component
    public Transform slotParent;  // Panel to hold all slots
    public ResourceType[] resources; // All resources to show

    private Dictionary<ResourceType, ResourceSlotUI> slots = new();

    void Start()
    {
        foreach (var res in resources)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            ResourceSlotUI slot = slotObj.GetComponent<ResourceSlotUI>();
            slot.SetResource(res);
            slots[res] = slot;
        }
    }

    void Update()
    {
        // Update all slots every frame (lightweight)
        foreach (var slot in slots.Values)
        {
            slot.UpdateAmount();
        }
    }
}