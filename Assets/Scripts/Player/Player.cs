using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f; // 反击持续时间

    public bool isBusy { get; private set; }

    [Header("Move info")]
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public float swordReturnImpact; // 剑返回冲击力

    [Header("Dash info")]
    public float dashSpeed = 28f;
    public float dashDuration = 0.2f;
    public float dashDir { get; private set; } // 为什么不是int

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region Statess
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; } // 反击状态
    public PlayerAimSwordState aimSwordState { get; private set; } // 瞄准剑状态（如果有的话）
    public PlayerCatchSwordState catchSwordState { get; private set; } // 拿剑状态（如果有的话）

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump"); // 对齐一下，这哥们儿有强迫症
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack"); // 反击状态
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword"); // 瞄准剑状态（如果有的话）
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword"); // 拿剑状态（如果有的话）
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance; // 获取技能管理器实例

        stateMachine.Initialize(idleState); // 初始化状态机

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput(); // 玩家在任何状态下都可以冲刺，这是合理的，因为冲刺是为了规避某些高伤害

    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword; // 分配新的剑
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState); // 切换到抓回剑状态
        Destroy(sword); // 销毁当前剑
        sword = null; // 清空剑引用
    }

    public IEnumerator BusyFor(float _seconds) 
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds); // 等待指定秒数

        isBusy = false;

    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger(); // 玩家可以调用攻击动画结束触发器

    private void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.CanUseSkill())
        {
            // 玩家可以控制冲刺方向，but如果玩家在空中也可以转向就好了
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            if (IsWallDetected() && !IsGroundDetected() )
                dashDir = -facingDir;

            stateMachine.ChangeState(dashState);
        }
    }


}
