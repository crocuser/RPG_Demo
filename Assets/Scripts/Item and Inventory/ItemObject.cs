using UnityEngine;

public class ItemObject : MonoBehaviour
{
    //private SpriteRenderer sr;

    [SerializeField] private ItemData itemData;

    private void OnValidate() // 在编辑器中修改脚本属性时调用
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item object - " + itemData.itemName;
    }

    //private void Start()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //    sr.sprite = itemData.icon;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
