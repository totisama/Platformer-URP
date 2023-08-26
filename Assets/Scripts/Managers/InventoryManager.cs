using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private GameObject itemPrefab;

    [HideInInspector]
    public int activeSlot = 0;
    Color defaultColor = new Color(1.0f, 1.0f, 1.0f);
    Color activeColor = new Color(0.71f, 0.71f, 0.71f);

    enum ActiveSlotIndex
    {
        Alpha1,
        Alpha2,
        Alpha3
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveSlot((int) ActiveSlotIndex.Alpha1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveSlot((int)ActiveSlotIndex.Alpha2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveSlot((int)ActiveSlotIndex.Alpha3);
        }
    }

    public void BuyItem(Item item)
    {
        if (GameManager.Instance.Coins < item.cost)
        {
            Debug.Log("No enough coins");

            return;
        }

        bool couldAdd = AddItem(item);

        if (!couldAdd)
        {
            Debug.Log("No enough space to buy potion");

            return;
        }

        GameManager.Instance.DecreaseCoins(item.cost);
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = slots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                SetActiveSlot(i);
                return true;
            }
        }

        return false;
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(itemPrefab, slot.transform);
        slot.transform.SetParent(newItem.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
    
    private void SetActiveSlot(int slot)
    {
        slots[activeSlot].GetComponent<Image>().color = defaultColor;

        activeSlot = slot;
        slots[activeSlot].GetComponent<Image>().color = activeColor;
    }

    public InventoryItem GetActiveSlotItem()
    {
        return slots[activeSlot].GetComponentInChildren<InventoryItem>();
    }

    //public void UseActiveSlotItem()
    //{
    //    Slider slider = slots[activeSlot].GetComponent<Slider>();

    //    if (!slider)
    //    {
    //        return;
    //    }
    //}
}
