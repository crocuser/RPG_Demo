using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType; // 装备槽类型

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item.data == null) // 如果槽位为空，直接返回（槽位为空，但是槽本身是存在的--类比抽屉-内容物）
            return;
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        ClearSlot();
    }
}
