using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = 1f; // 持续时间
        player.SetVelocity(5 * -player.facingDir, player.jumpForce); //水平反方向向上跳
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 )
            stateMachine.ChangeState(player.airState); // 蹬墙结束进入空气状态，空气状态会判断是否撞墙
        else if (stateTimer < 0.5f && xInput != player.facingDir)
        {
            player.SetVelocity(xInput * player.moveSpeed, player.jumpForce); // 在蹬墙状态下，允许玩家水平移动
            if (!player.IsWallDetected())
                stateMachine.ChangeState(player.airState); // 如果没有撞墙，进入空气状态
        }

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState); // 撞墙墙则滑墙

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState); // 落地进入空闲
    }
}
