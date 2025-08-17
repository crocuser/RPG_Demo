using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f; // 飞行时间
    private bool skillUsed; // 技能是否使用过

    private float defaultGravityScale; // 默认重力缩放值
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravityScale = rb.gravityScale; // 保存默认重力缩放值

        skillUsed = false; // 初始化技能使用状态
        stateTimer = flyTime; // 设置状态计时器为飞行时间
        rb.gravityScale = 0; // 禁用重力
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = defaultGravityScale; // 恢复默认重力缩放值
        player.MakeTransparent(false); // 恢复玩家透明度
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 15); // 向上飞行
        }

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f); // 缓慢向下飞行

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true; // 标记技能已使用
            }
        }

        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState); // 如果黑洞技能完成，切换到空气状态
    }
}
