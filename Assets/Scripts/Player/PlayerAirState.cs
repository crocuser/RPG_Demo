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

        // y��û���ٶȽ������״̬
        //if (rb.linearVelocity.y == 0)
        if (player.IsGroundDetected()) // ����״̬�У����ӵ�ʱ��ת��Ϊ����״̬
        {
            //player.SetVelocity(0, rb.linearVelocity.y); // ��Ҫ�Ż�
            stateMachine.ChangeState(player.idleState);
        }

        //@crocuser_xh: ��ӿ���ת�򲢻���ٶ�
        if (xInput != 0)
            player.SetVelocity(xInput * player.moveSpeed /* 0.8f*/, rb.linearVelocity.y); // Ŷ�����½ڿ����ϸ���˿����ƶ����Ͷ��Ȩ�ء�

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
    }
}