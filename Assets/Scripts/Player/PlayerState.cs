using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer; // 定时器
    protected bool triggerCalled; // 触发器标志

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        // 构造函数
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // 进入，退出，持续，只需判断是否满足条件，不用手动处理动画间的转换关系
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        // 将变量名和变量值传给处理函数，用于设置，简洁，复用性，可读性，非常精妙!!!
        rb = player.rb;

        triggerCalled = false; // 进入状态后，标记未结束
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // 更新定时器，每个状态都会用到定时器

        // 删除某行代码快捷键：ctrl+L
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.linearVelocity.y); // 刚体有重力，y轴速度会实时更新
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
