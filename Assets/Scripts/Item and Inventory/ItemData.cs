using UnityEngine;

public enum ItemType
{
    Material, // 材料 
    Equipment, // 装备
    Quest, // 任务物品
    Miscellaneous // 杂项
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropChance;
}
