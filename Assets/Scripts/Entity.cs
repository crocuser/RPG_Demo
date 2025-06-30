using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    //#region �� C# ��Ԥ����ָ������ڴ����ж���һ�����۵������򣬷��������֯��ctrl+k;ctrl+s
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EnityFX fx { get; private set; }

    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection; // ���˷���
    [SerializeField] protected float knockbackDuration; // ���˳���ʱ��
    protected bool isKnocked; // �Ƿ����

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius; // �������뾶
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;


    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fx = GetComponentInChildren<EnityFX>(); // ��ȡ����� EnityFX
        anim = GetComponentInChildren<Animator>(); // ��ȡ����� Animator
        rb = GetComponent<Rigidbody2D>();

    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX"); // ���� EnityFX �е���˸��ЧЭ��
        StartCoroutine("HitKnockback"); // ���û���Э��
        Debug.Log(gameObject.name + " is damaged!");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true; // ����Ϊ������״̬

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration); // �ȴ����˳���ʱ��
        isKnocked = false;
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return; // ��������ˣ��������ٶ�

        rb.linearVelocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {

        if (isKnocked)
            return; // ��������ˣ��������ٶ�

        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity); // �����ٶ�ʱ�����з�ת���ƣ�̫������
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround); // �����⣬lambda���ʽ
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround); // ǽ����

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
        // ��ת����
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion
}
