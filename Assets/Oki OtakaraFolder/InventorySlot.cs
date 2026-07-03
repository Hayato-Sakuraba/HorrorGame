using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage;

    public void SetItem(Otakara item)
    {
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void Clear()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
    }
}