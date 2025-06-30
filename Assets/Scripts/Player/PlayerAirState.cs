using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // y轴没有速度进入空闲状态
        //if (rb.linearVelocity.y == 0)
        if (player.IsGroundDetected()) // 空气状态中，当接地时，转换为空闲状态
        {
            //player.SetVelocity(0, rb.linearVelocity.y); // 不要脚滑
            stateMachine.ChangeState(player.idleState);
        }

        //@crocuser_xh: 添加空中转向并获得速度
        if (xInput != 0)
            player.SetVelocity(xInput * player.moveSpeed /* 0.8f*/, rb.linearVelocity.y); // 哦，在下节课里老哥加了空中移动，就多个权重。

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
    }
}