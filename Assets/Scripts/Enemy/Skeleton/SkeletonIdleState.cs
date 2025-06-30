using UnityEngine;

public class SkeletonIdleState : SKeletonGroundState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Á¢¶¨¹Û²ìing
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
