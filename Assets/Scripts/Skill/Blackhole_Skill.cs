using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private int amountAttack; // 克隆体的攻击次数，如会随着技能等级提升而增加
    [SerializeField] private float cloneAttackCooldown; // 克隆体攻击的冷却时间
    [SerializeField] private float blackholeDuration; // 黑洞持续时间
    [Space]
    [SerializeField] private GameObject blackholePrefab; // 黑洞预制体
    [SerializeField] private float maxSize; // 黑洞的最大尺寸
    [SerializeField] private float growSpeed; // 黑洞的增长速度
    [SerializeField] private float shrinkSpeed; // 黑洞的缩小速度

    Blackhole_Skill_Controller currentBlackhole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountAttack, cloneAttackCooldown, blackholeDuration); // 设置黑洞的属性
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null; // 清除当前黑洞引用
            return true; // 黑洞技能已完成，可以退出状态
        }

        return false; // 黑洞技能未完成，不能退出状态
    }

}
