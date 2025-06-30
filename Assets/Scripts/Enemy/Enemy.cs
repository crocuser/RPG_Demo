using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer; // 检测玩家

    [Header("Stunned info")]
    public float stunnedDuration; // 眩晕持续时间
    public Vector2 stunnedKnockbackDirection; // 眩晕时的击退方向
    protected bool canBeStunned = false; // 是否可以被眩晕
    [SerializeField] protected GameObject counterImage; // 是玩家可以进行反击击晕敌人时的图像提示

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime; // 战斗时间，敌人会在这个时间内攻击玩家

    [Header("Attack info")]
    public float attackDistance; // 攻击距离
    public float attackCooldown; // 攻击冷却时间
    [HideInInspector] public float lastTimeAttacked; // 最后的攻击时间，隐藏掉

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        //RaycastHit2D rch = IsPlayerDetected();
        //if (rch == true)
        //    Debug.Log(rch.collider.gameObject.name + " I SEE");
    }

    public virtual void OpenCounterAttackWindow() // 打开反击击晕敌人时的图像提示
    {
        canBeStunned = true;
        counterImage.SetActive(true); // 显示反击图像提示
    }

    public virtual void CloseCounterAttackWindow() // 关闭反击击晕敌人时的图像提示
    {
        canBeStunned = false;
        counterImage.SetActive(false); // 隐藏反击图像提示
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow(); // 关闭反击图像提示
            return true;
        }

        return false;
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer); // 源，方向，距离，图层，使用墙作为源不会存在隔着墙看见玩家的情况。


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
