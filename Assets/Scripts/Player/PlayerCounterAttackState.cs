using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    // 安装插件 Force UTF-8(No BOM)，防Unity预览中文乱码
    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration; // 设置反击持续时间
        player.anim.SetBool("SuccessfulCounterAttack", false); // 确保反击成功标志为false
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity(); // 设置玩家速度为0，防止在反击状态中移动

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned()) // 检查敌人是否可以被眩晕
                {
                    stateTimer = 10; // 设置一个较长的时间，确保反击动画可以完成
                    player.anim.SetBool("SuccessfulCounterAttack", true); // 设置反击成功标志为true，播放反击成功动画
                }
            }
        }

        if (stateTimer < 0 || triggerCalled) // 如果计时器小于0(反击失败)或者 动画触发器被调用(反击成功)
        {
            // 反击结束，切换回空闲状态
            stateMachine.ChangeState(player.idleState);
        }
    }
}
