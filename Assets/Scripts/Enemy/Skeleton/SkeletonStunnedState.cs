using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // ��ʱ�ظ����ã�InvokeRepeating("������", ��ʼ�ӳ�, ���ü��);
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunnedDuration;
        
        enemy.rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunnedKnockbackDirection.x, enemy.stunnedKnockbackDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedBlink", 0); // ȡ��ѣ��ʱ�ĺ�ɫ��˸Ч��
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemy.stateMachine.ChangeState(enemy.idleState); // ѣ�ν������ص�idle״̬
    }
}
