using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType; // 装备槽类型

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }
}
