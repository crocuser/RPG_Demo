using UnityEngine;

public enum statType
{
    strength, // 力量--暴击伤害
    agility, // 敏捷--闪避速度
    intelligence, // 智力--魔法伤害
    vitality, // 体力--生命值
    damage,//暴击伤害
    critChance, // 暴击率
    critPower, // 暴击倍率
    maxHealth, // 最大生命值
    armor, // 护甲--物理伤害减免
    evasion, // 闪避--闪避率
    magicResistance, // 魔抗--魔法伤害减免
    fireDamage, // 火焰伤害
    iceDamage, // 冰霜伤害
    lightningDamage // 闪电伤害
}

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Objects/Item effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private statType buffType; // 增加属性的类型
    [SerializeField] private int buffAmount; // 增加的属性值
    [SerializeField] private float buffDuration; // 增加属性的持续时间

    public override void ExecuteEffect(Transform _enemyPosiition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        return buffType switch
        {
            statType.strength => stats.strength,
            statType.agility => stats.agility,
            statType.intelligence => stats.intelligence,
            statType.vitality => stats.vitality,
            statType.damage => stats.damage,
            statType.critChance => stats.critChance,
            statType.critPower => stats.critPower,
            statType.maxHealth => stats.maxHealth,
            statType.armor => stats.armor,
            statType.evasion => stats.evasion,
            statType.magicResistance => stats.magicResistance,
            statType.fireDamage => stats.fireDamage,
            statType.iceDamage => stats.iceDamage,
            statType.lightningDamage => stats.lightningDamage,
            _ => throw new System.ArgumentOutOfRangeException(nameof(buffType), $"无效的属性类型：{buffType}")
        };
    }
}
