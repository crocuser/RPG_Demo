using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowTooltip(ItemData_Equipment item)
    {
        itemNameText.text = item.itemName;
        itemTypeText.text = item.itemType.ToString();
        itemDescriptionText.text = item.GetDescription();

        gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
