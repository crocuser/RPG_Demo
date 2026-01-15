using UnityEngine;

public enum EquipmentType
{
    Weapon, // 武器
    Armor, // 盔甲
    Amulet, // 护符
    Flask // 药瓶
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
