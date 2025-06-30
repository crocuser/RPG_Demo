using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time; // ��¼���Ĺ���ʱ��
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity(); // ����ʱ����Ҫ�ƶ�

        if (triggerCalled) // �������ص�ս��״̬��Ѱ�У�
            stateMachine.ChangeState(enemy.battleState);
    }

}
