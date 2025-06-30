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

        // ��ǽ�ϣ����¿ո����ǽ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return; //����������Ὣˮƽ�ٶ���Ϊ�㡣
        }

        // ����ǽ�ϣ���������ļ�����Ҫ��ǽ������
        if (xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState); // û���⣬idle������ж��Ƿ�ӵأ����ӵأ�ת����״̬

        if (yInput < 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // ��Ұ�ס���¼������ø���
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);

        // �ӵ��˾ʹ�ǽ������
        if (player.IsGroundDetected())
        {
            player.Flip(); //��ǽ��غ�ת��
            stateMachine.ChangeState(player.idleState);
        }
    }
}
