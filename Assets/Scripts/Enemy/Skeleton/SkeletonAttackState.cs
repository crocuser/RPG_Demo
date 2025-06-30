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

        enemy.lastTimeAttacked = Time.time; // 记录最后的攻击时间
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity(); // 攻击时，不要移动

        if (triggerCalled) // 攻击完后回到战斗状态（寻敌）
            stateMachine.ChangeState(enemy.battleState);
    }

}
