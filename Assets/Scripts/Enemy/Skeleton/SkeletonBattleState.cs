using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _startMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _startMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform; // 获取玩家的Transform
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime; // 进入战斗状态，设置战斗时间

            // 与玩家的距离小于攻击距离，则敌人停下了进行攻击
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack()) // 过了攻击冷却时间
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState); // 如果没有检测到玩家，且战斗时间已过，则切换到闲置状态
        }

        // 移动方向判断
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        // 设置速度
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);

    }


    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time; // 过了冷却时间，更新时间，返回true
            return true;
        }
        return false;
    }

}
