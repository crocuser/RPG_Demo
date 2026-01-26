using NUnit.Framework;
using System.Collections.Generic;
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

    // 直接复制 CharacterStats 里的属性
    [Header("Major stats")]
    public int strength; // 力量--暴击伤害
    public int agility; // 敏捷--闪避速度
    public int intelligence; // 智力--魔法伤害
    public int vitality; // 体力--生命值

    [Header("Offensive stats")]
    public int damage;
    public int critChance; // 暴击率
    public int critPower; // 暴击倍率

    [Header("Defensive stats")]
    public int maxHealth; // 最大生命值
    public int armor; // 护甲--物理伤害减免
    public int evasion; // 闪避--闪避率
    public int magicResistance; // 魔抗--魔法伤害减免

    [Header("Magic stats")]
    public int fireDamage; // 火焰伤害
    public int iceDamage; // 冰霜伤害
    public int lightningDamage; // 闪电伤害

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials; // 合成材料
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }

    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);

    }
}
