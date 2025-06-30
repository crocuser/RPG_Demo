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

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed = 28f;
    public float dashDuration = 0.2f;
    public float dashDir { get; private set; } // 为什么不是int



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
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 初始化状态机

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput(); // 玩家在任何状态下都可以冲刺，这是合理的，因为冲刺是为了规避某些高伤害

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
        dashUsageTimer-=Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown; // 更新冷却时间
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
