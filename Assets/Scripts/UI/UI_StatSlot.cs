using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI_Pages uiPages;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;
    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValueUI();

        uiPages = GetComponentInParent<UI_Pages>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();


            if (statType == StatType.damage)
                statValueText.text += " (+" + playerStats.GetStat(StatType.strength).GetValue() + ")";

            if (statType == StatType.critPower)
                statValueText.text += " (+" + playerStats.GetStat(StatType.strength).GetValue() + ")";

            if (statType == StatType.evasion)
                statValueText.text += " (+" + playerStats.GetStat(StatType.agility).GetValue() + ")";

            if (statType == StatType.critChance)
                statValueText.text += " (+" + playerStats.GetStat(StatType.agility).GetValue() + ")";

            if (statType == StatType.fireDamage)
                statValueText.text += " (+" + playerStats.GetStat(StatType.intelligence).GetValue() + ")";

            if (statType == StatType.iceDamage)
                statValueText.text += " (+" + playerStats.GetStat(StatType.intelligence).GetValue() + ")";

            if (statType == StatType.lightningDamage)
                statValueText.text += " (+" + playerStats.GetStat(StatType.intelligence).GetValue() + ")";

            if (statType == StatType.magicResistance)
                statValueText.text += " (+" + playerStats.GetStat(StatType.intelligence).GetValue() * 3 + ")";

            if (statType == StatType.maxHealth)
                statValueText.text += " (+" + playerStats.GetStat(StatType.vitality).GetValue() * 5 + ")";

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiPages.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiPages.statTooltip.HideStatTooltip();
    }
}
