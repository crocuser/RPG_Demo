using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown; // 技能冷却时间
    protected float cooldownTimer; // 技能冷却计时器

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update() // 嗯，注意Update首字母大写，以及Unity自带的方法是蓝色标识
    {
        cooldownTimer -= Time.deltaTime; // 减少冷却计时器
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill(); // 使用技能
            cooldownTimer = cooldown; // 重置冷却计时器
            return true; // 可以使用技能
        }

        // Debug.Log("Skill is on cooldown");
        return false; // 技能冷却中，不能使用技能
    }

    public virtual void UseSkill()
    {
        // do some skill spesific things
    }
}
