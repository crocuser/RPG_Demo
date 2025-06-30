using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = 1f; // ����ʱ��
        player.SetVelocity(5 * -player.facingDir, player.jumpForce); //ˮƽ������������
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 )
            stateMachine.ChangeState(player.airState); // ��ǽ�����������״̬������״̬���ж��Ƿ�ײǽ
        else if (stateTimer < 0.5f && xInput != player.facingDir)
        {
            player.SetVelocity(xInput * player.moveSpeed, player.jumpForce); // �ڵ�ǽ״̬�£��������ˮƽ�ƶ�
            if (!player.IsWallDetected())
                stateMachine.ChangeState(player.airState); // ���û��ײǽ���������״̬
        }

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState); // ײǽǽ��ǽ

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState); // ��ؽ������
    }
}
