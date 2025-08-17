using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateClone(player.transform, player.dashDir, Vector3.zero); // 创建冲刺克隆

        stateTimer = player.dashDuration; // 冲刺时间
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y); // 退出冲刺时重置速度
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 || (player.IsWallDetected() && player.IsGroundDetected()))
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }//冲刺结束或撞墙，回到空闲状态

        player.SetVelocity(player.dashSpeed * player.dashDir, 0); // 给予冲刺速度，并且冲刺时y轴速度设置为0，但是刚体的重力一直存在，不会有冲刺完了后还向上飞的情况。

    }
}
