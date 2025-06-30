using UnityEngine;

public class SKeletonGroundState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;

    public SKeletonGroundState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(player.transform.position, enemy.transform.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
