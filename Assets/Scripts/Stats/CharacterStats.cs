using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
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

public class CharacterStats : MonoBehaviour
{
    private EnityFX fx;

    [Header("Major stats")]
    public Stat strength; // 力量--暴击伤害：增加基础伤害和暴击倍率
    public Stat agility; // 敏捷--闪避速度：增加闪避率和暴击率
    public Stat intelligence; // 智力--魔法伤害：增加基础魔法伤害和魔抗
    public Stat vitality; // 体力--生命值：增加最大生命值

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
    [SerializeField] private float ailmentDuration = 4; // 负面状态持续时间
    private float igniteTimer; // 燃烧持续时间
    private float chillTimer; // 冰冻持续时间
    private float shockTimer; // 电击持续时间


    private float igniteDamageCooldown = .3f; // 每0.3掉血一次
    private float igniteDamageTimer; // 计时器
    private int igniteDamage; // 每次掉血值
    [SerializeField] private GameObject shockStrikePrefab; // 闪电链预制体
    private int shockDamage; // 闪电链伤害值

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150); // 默认暴击伤害150%
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EnityFX>();

        //Debug.Log("start character stats");
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

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        // _modifier 是增益的数值，比如增加10点力量
        // _duration 是增益持续的时间，比如10秒
        // _statToModify 是要被修改的属性，比如力量、敏捷等

        // 施加增益，持续一段时间后移除增益，使用协程来处理增益持续时间
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        
        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats)) // 目标闪避成功
            return;

        DoPhysicalDamage(_targetStats);

        DoMagicDamage(_targetStats); // 主武器造成伤害后，可附加一次魔法伤害和状态效果

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

    #region Magical damage and ailements
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int maxDamage = Mathf.Max(_fireDamage, _iceDamage, _lightningDamage);

        int totalMagicDamage = maxDamage + intelligence.GetValue(); // 智力增基础魔法伤害（数值）

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        //Debug.Log("魔法伤害：" + totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);


        if (maxDamage <= 0)
            return; // 如果没有任何魔法伤害，就不施加状态

        bool flowControl = AttemptToApplyAilement(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
        if (!flowControl) // 未成功施加状态
        {
            return;
        }

    }

    private bool AttemptToApplyAilement(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
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
                return false;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("冰冻");
                return false;
            }
            if (Random.value < 1f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("电击");
                return false;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f)); // 点燃每次掉血为火焰伤害的20%

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .5f)); // 闪电链伤害为闪电伤害的50%

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        return true;
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        //if (isIgnited || isChilled || isShocked)
        //    return;// 不能叠加状态
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled; // 电击状态可以叠加

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            igniteTimer = ailmentDuration; // 点燃时间

            fx.IgniteFxFor(ailmentDuration); // 触发点燃特效
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chillTimer = ailmentDuration; // 冰冻时间

            float slowPercentage = .5f;
            GetComponent<Entity>().SlowEnityBy(slowPercentage, ailmentDuration); // 冰冻时减速

            fx.ChillFxFor(ailmentDuration); // 触发冰冻特效
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                HitNearestTargetWithShockStrike(); // 已经被电击，则向最近的敌人释放一次闪电链

            }
        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            //Debug.Log("Take burn damage: " + igniteDamage);

            DecreaseHealthBy(igniteDamage); // 持续掉血，调用UI更新生命值

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void ApplyShock(bool _shock)
    {
        isShocked = _shock;
        shockTimer = ailmentDuration; // 电击时间

        fx.ShockFxFor(ailmentDuration); // 触发电击特效
    }

    private void HitNearestTargetWithShockStrike()
    {
        // 如果已经被电击，则向最近的敌人释放一次闪电链（由敌人自己出发下一次电击）
        // 从目前效果来看时，处于电击中的敌人，再次受到闪电攻击，则给它进行了一次余电袭击
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25); // 检测范围内的所有碰撞体

        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        Transform closestEnemy = null; // 初始化最近的敌人为null

        foreach (var hit in colliders)
        {
            if (GetComponent<Player>() != null)
            {
                closestEnemy = hit.GetComponent<Player>()?.transform; // 被攻击的玩家也会触发闪电链
                break;
            }

            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1) // 如果碰撞体是敌人
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position); // 计算与克隆体的距离
                if (distanceToEnemy <= closestDistance) // 如果距离小于当前最近距离
                {
                    closestDistance = distanceToEnemy; // 更新最近距离
                    closestEnemy = hit.transform; // 更新最近的敌人
                }

            }

        }
        if (closestEnemy == null) // 如果没有找到其他敌人，就释放给自己
            closestEnemy = transform;

        if (closestEnemy != null)
        {
            // 释放闪电链
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion
    public virtual void TakeDamage(int _damage)
    {
        if (_damage == 0)
            return;

        // 从敌人的角度，受到伤害
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact(); // 调用 Entity 中的受击反馈方法
        fx.StartCoroutine("FlashFX"); // 调用 EnityFX 中的闪烁特效协程

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        onHealthChanged?.Invoke();
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    #region Stat calculations
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f); // 被冰冻时，护甲减少20%
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // 确保伤害不为负数
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3); // 智力也增魔抗（数值）
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue); // 确保伤害不为负数
        return totalMagicDamage;
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
    #endregion

    public Stat GetStat(StatType _statType)
    {
        return _statType switch
        {
            // switch的新写法，根据_statType的值返回对应的Stat属性统计
            StatType.strength => strength, // 在角色统计里，这个属性名就代表这个属性统计
            StatType.agility => agility,
            StatType.intelligence => intelligence,
            StatType.vitality => vitality,
            StatType.damage => damage,
            StatType.critChance => critChance,
            StatType.critPower => critPower,
            StatType.maxHealth => maxHealth,
            StatType.armor => armor,
            StatType.evasion => evasion,
            StatType.magicResistance => magicResistance,
            StatType.fireDamage => fireDamage,
            StatType.iceDamage => iceDamage,
            StatType.lightningDamage => lightningDamage,
            _ => throw new System.ArgumentOutOfRangeException(nameof(_statType), $"无效的属性类型：{_statType}")
        };
    }
}
