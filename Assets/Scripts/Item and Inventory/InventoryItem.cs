using System;
using UnityEngine;

// 可序列化，在Inspector面板显示
[Serializable]
public class InventoryItem
{
    // 物品数据
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
