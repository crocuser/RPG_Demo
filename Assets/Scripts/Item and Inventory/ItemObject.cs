using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    // 已改成预制件，
    //private void OnValidate() // 在编辑器中修改脚本属性时调用
    //{
       
    //}

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _veiocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _veiocity;

        SetupVisuals();
    }
    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
