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

        // 定时重复调用：InvokeRepeating("方法名", 初始延迟, 调用间隔);
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemy.stunnedDuration;
        
        enemy.rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunnedKnockbackDirection.x, enemy.stunnedKnockbackDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelRedBlink", 0); // 取消眩晕时的红色闪烁效果
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemy.stateMachine.ChangeState(enemy.idleState); // 眩晕结束，回到idle状态
    }
}
