using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration; // ���ʱ��
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y); // �˳����ʱ�����ٶ�
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 || (player.IsWallDetected() && player.IsGroundDetected()))
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }//��̽�����ײǽ���ص�����״̬

        player.SetVelocity(player.dashSpeed * player.dashDir, 0); // �������ٶȣ����ҳ��ʱy���ٶ�����Ϊ0�����Ǹ��������һֱ���ڣ������г�����˺����Ϸɵ������

    }
}
