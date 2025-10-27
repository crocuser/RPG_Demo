using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; // 力量--暴击伤害
    public Stat agility; // 敏捷--闪避速度
    public Stat intelligence; // 智力--魔法伤害
    public Stat vitality; // 体力--生命值

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance; // 暴击率
    public Stat critPower; // 暴击倍率

    [Header("Defensive stats")]
    public Stat maxHealth; // 最大生命值
    public Stat armor; // 护甲--物理伤害减免
    public Stat evasion; // 闪避--闪避率
    public Stat magicResistance; // 魔抗--魔法伤害减免

    [Header("Magic stats")]
    public Stat fireDamage; // 火焰伤害
    public Stat iceDamage; // 冰霜伤害
    public Stat lightningDamage; // 闪电伤害

    public bool isIgnited; // 是否被点燃，会持续掉血
    public bool isChilled; // 是否被冰冻，会减少护甲
    public bool isShocked; // 是否被电击，会减少闪避

    //造成魔法伤害，施加负面状态

    private float igniteTimer; // 燃烧持续时间
    private float chillTimer; // 冰冻持续时间
    private float shockTimer; // 点击持续时间


    private float igniteDamageCooldown = .3f; // 每0.3掉血一次
    private float igniteDamageTimer; // 计时器
    private int igniteDamage; // 每次掉血值

    public int currentHealth;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150); // 默认暴击伤害150%
        currentHealth = GetMaxHealthValue();
        Debug.Log("start character stats");
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (igniteTimer < 0)
            isIgnited = false;

        if (chillTimer < 0)
            isChilled = false;

        if (shockTimer < 0)
            isShocked = false;

        if (igniteDamageTimer < 0 && isIgnited)
        {
            //Debug.Log("Take burn damage: " + igniteDamage);

            DecreaseHealthBy(igniteDamage); // 持续掉血，调用UI更新生命值

            if (currentHealth <= 0)
            { 
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        //DoPhysicalDamage(_targetStats);

        DoMagicDamage(_targetStats);

    }

    public virtual void DoPhysicalDamage(CharacterStats _targetStats)
    {
        // 从玩家的角度，对敌人造成的伤害
        int totalPhysicalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            // strength 既增基础伤害（数值），又增暴击伤害（倍率）
            totalPhysicalDamage = CalculateCritDamage(totalPhysicalDamage);
        }

        totalPhysicalDamage = CheckTargetArmor(_targetStats, totalPhysicalDamage);
        _targetStats.TakeDamage(totalPhysicalDamage);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue(); // 智力增基础魔法伤害（数值）

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        //Debug.Log("魔法伤害：" + totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return; // 如果没有任何魔法伤害，就不施加状态

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("点燃");
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("冰冻");
                return;
            }
            if (Random.value < 1f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("电击");
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // 点燃每次掉血为火焰伤害的20%

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3); // 智力也增魔抗（数值）
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue); // 确保伤害不为负数
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;// 不能叠加状态

        if (_ignite)
        {
            isIgnited = _ignite;
            igniteTimer = 3f; // 点燃持续3秒
        }

        if (_chill)
        {
            isChilled = _chill;
            chillTimer = 3f; // 冰冻持续3秒
        }

        if (_shock)
        {
            isShocked = _shock;
            shockTimer = 3f; // 电击持续3秒
        }
    }
    public virtual void TakeDamage(int _damage)
    {
        // 从敌人的角度，受到伤害
        DecreaseHealthBy(_damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }
    protected virtual void Die()
    {

    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f); // 被冰冻时，护甲减少20%
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // 确保伤害不为负数
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        // 返回是否闪避成功。这不就是空洞骑士的无忧旋律或者磁石股子吗！
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20; // 自己被电击时，目标的闪避率增加20%

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue(); // 暴击率受敏捷影响
        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f; // 暴击伤害受力量影响

        float critDamage = _damage * totalCritPower;

        // Debug.Log("暴击率和暴击伤害：" + totalCritPower + "、" + critDamage);

        return Mathf.RoundToInt(critDamage); // 四舍五入取整
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
