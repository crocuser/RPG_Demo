using UnityEditorInternal;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // 在墙上，按下空格，则蹬墙跳
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return; //跳出，下面会将水平速度设为零。
        }

        // 贴在墙上，按反方向的键，想要从墙上下来
        if (xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState); // 没问题，idle里面会判断是否接地，不接地，转空气状态

        if (yInput < 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // 玩家按住向下键，滑得更快
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);

        // 接地了就从墙上下来
        if (player.IsGroundDetected())
        {
            player.Flip(); //滑墙落地后转向
            stateMachine.ChangeState(player.idleState);
        }
    }
}
