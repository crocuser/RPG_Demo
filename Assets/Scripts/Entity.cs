using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    //#region 是 C# 的预处理指令，用于在代码中定义一个可折叠的区域，方便代码组织。ctrl+k;ctrl+s
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EnityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection; // 击退方向
    [SerializeField] protected float knockbackDuration; // 击退持续时间
    protected bool isKnocked; // 是否击退

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius; // 攻击检测半径
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;


    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    public System.Action onFlipped; // 翻转时的回调事件


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // 获取子组件 SpriteRenderer
        anim = GetComponentInChildren<Animator>(); // 获取子组件 Animator
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EnityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEnityBy(float _slowPercentage, float _slowDuration) // 减速方法
    {
        // 留给子类实现
    }
    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1f; // 恢复默认速度
    }
    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX"); // 调用 EnityFX 中的闪烁特效协程
        StartCoroutine("HitKnockback"); // 调用击退协程
        //Debug.Log(gameObject.name + " is damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true; // 设置为被击退状态

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration); // 等待击退持续时间
        isKnocked = false;
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return; // 如果被击退，则不设置速度

        rb.linearVelocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {

        if (isKnocked)
            return; // 如果被击退，则不设置速度

        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity); // 设置速度时，进行翻转控制，太优雅了
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround); // 地面检测，lambda表达式
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround); // 墙体检测

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        // 翻转操作
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke(); // 触发翻转事件
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear; // 设置透明
        else
            sr.color = Color.white;
    }

    public virtual void Die()
    {
        
    }
}
