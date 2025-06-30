using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter; // 连击次数

    private float lastTimeAttacked; // 最后攻击的时间
    private float comboWindow = 1; // 连击的时间间隔
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        // 重置连击次数
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter); // 绑定计数器

        // 记录玩家输入的攻击方向
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y +rb.linearVelocity.y); // 攻击时给予速度，绝对控制，控制停，控制位移

        stateTimer = .1f; // 有一点惯性

        //player.anim.speed = 3f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f); // 攻击后摇

        comboCounter++;
        lastTimeAttacked = Time.time;

        //player.anim.speed = 1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity(); // 解决滑步问题

        // 结束动画触发，进入空闲状态
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
