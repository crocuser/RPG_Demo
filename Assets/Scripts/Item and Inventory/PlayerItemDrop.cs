using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToDropStashItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> currentEquipment = new List<InventoryItem>(inventory.GetEquipment());
        List<InventoryItem> currentStash = new List<InventoryItem>(inventory.stashItems);

        for (int i = 0; i < currentEquipment.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToLoseItems)
            {
                DropItem(currentEquipment[i].data);
                inventory.UnequipItem(currentEquipment[i].data as ItemData_Equipment, true);
            }
        }

        for (int i = 0; i < currentStash.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToDropStashItems)
            {
                DropItem(currentStash[i].data);
                inventory.RemoveItem(currentStash[i].data);
            }
        }
    }
}
