using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    // 物品列表
    public List<InventoryItem> inventoryItems; // 物品实例列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; // 物品数据到物品实例的映射

    public List<InventoryItem> stashItems; // 储藏实例列表
    public Dictionary<ItemData, InventoryItem> stashDictionary; // 储藏数据到储藏实例的映射

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent; // UI物品槽的父物体
    [SerializeField] private Transform stashSlotParent; // UI储藏槽的父物体

    private UI_ItemSlot[] inventoryItemSlot; // 物品槽数组
    private UI_ItemSlot[] stashItemSlot; // 储藏槽数组

    private void Awake()
    {
        // 单例模式：没有则创建，有则销毁多余实例
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取所有物品槽组件
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取所有储藏槽组件
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventoryItems[i]);
        }

        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stashItems[i]);
        }
    }
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            // 如果物品已存在，增加堆叠数量
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else 
            {
                value.RemoveStack();
            }
        }

        if(stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;
            
    //        RemoveItem(newItem);
    //    }
    //}
}
