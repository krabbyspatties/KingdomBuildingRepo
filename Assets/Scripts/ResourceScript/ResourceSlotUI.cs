using UnityEngine;
using UnityEngine.UI;

public class ResourceSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TMPro.TextMeshProUGUI amountText; // Use TMP for better text

    private ResourceType resource;

    public void SetResource(ResourceType res)
    {
        resource = res;
        if (iconImage != null)
            iconImage.sprite = res.icon;
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        if (resource != null && amountText != null)
        {
            int amount = ResourceManager.Instance.Get(resource);
            amountText.text = amount.ToString();
        }
    }
}