using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> startingItems; // 初始装备列表

    public List<InventoryItem> equipmentItems; // 装备实例列表
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary; // 装备数据到装备实例的映射

    public List<InventoryItem> inventoryItems; // 物品实例列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; // 物品数据到物品实例的映射

    public List<InventoryItem> stashItems; // 储藏实例列表
    public Dictionary<ItemData, InventoryItem> stashDictionary; // 储藏数据到储藏实例的映射

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent; // UI物品槽的父物体
    [SerializeField] private Transform stashSlotParent; // UI储藏槽的父物体
    [SerializeField] private Transform equipmentSlotParent; // UI装备槽的父物体

    private UI_ItemSlot[] inventoryItemSlot; // 物品槽数组
    private UI_ItemSlot[] stashItemSlot; // 储藏槽数组
    private UI_EquipmentSlot[] equipmentSlot; // 装备槽数组

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

        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取所有物品槽组件
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>(); // 获取所有储藏槽组件
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>(); // 获取所有装备槽组件

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        // 使用装备时，检查是否已有相同类型的装备，如果已使用则替换

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
                break;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipmentItems.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers(); // 添加属性加成

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove, bool isDeadDrop = false)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            if (isDeadDrop)
            {
                PlayerDeadDrop();
                Debug.Log("死亡爆装备");
            }
            equipmentItems.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers(); // 移除属性加成
            
        }
    }

    private void PlayerDeadDrop()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].ClearSlot();
                }
            }
        }
    }

    private void UpdateSlotUI()
    {
        // 更新装备槽UI
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        // 更新物品槽和储藏槽UI
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].ClearSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].ClearSlot();
        }

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

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToUsed = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++) // 遍历所需材料
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                // 检查材料数量是否足够
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials to craft " + _itemToCraft.itemName);
                    return false;
                }
                else
                {
                    materialsToUsed.Add(_requiredMaterials[i]);
                }
            }
            else 
            {
                // 材料不存在于储藏中
                Debug.Log("Missing materials to craft " + _itemToCraft.itemName);
                return false;
            }
        }

        for (int i = 0; i < materialsToUsed.Count; i++)
        {
            // 使用材料
            for (int j = 0; j < materialsToUsed[i].stackSize; j++)
                RemoveItem(materialsToUsed[i].data);
        }

        // 制作物品
        AddItem(_itemToCraft);
        Debug.Log("Crafted " + _itemToCraft.itemName);

        return true;
    }

    public List<InventoryItem> GetEquipment() => equipmentItems;
    public List<InventoryItem> GetInventory() => stashItems;

}
