using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter; // ��������

    private float lastTimeAttacked; // ��󹥻���ʱ��
    private float comboWindow = 1; // ������ʱ����
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        // ������������
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter); // �󶨼�����

        // ��¼�������Ĺ�������
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y +rb.linearVelocity.y); // ����ʱ�����ٶȣ����Կ��ƣ�����ͣ������λ��

        stateTimer = .1f; // ��һ�����

        //player.anim.speed = 3f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f); // ������ҡ

        comboCounter++;
        lastTimeAttacked = Time.time;

        //player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity(); // �����������

        // ���������������������״̬
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
