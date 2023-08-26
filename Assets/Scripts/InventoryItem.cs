using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class InventoryItem : MonoBehaviour
{
    private Item item;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
    }

    public ActionType GetItemType()
    {
        return item.type;
    }
    
    public float GetItemDuration()
    {
        return item.duration;
    }
    
    public int GetItemExtraHealth()
    {
        return item.extraHealth;
    }
}
