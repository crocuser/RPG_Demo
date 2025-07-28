using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    [SerializeField] private float returnSpeed = 12f; // 返回玩家手中的速度
    private bool canRotate = true;
    private bool isReturning = false; // 是否正在返回玩家手中
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player; // 设置玩家引用，便于后续操作
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        //rb.bodyType = RigidbodyType2D.Dynamic; // 恢复物理属性，使剑受重力和碰撞影响
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有约束，防止物理交互
        transform.parent = null; // 解除与目标的父子关系，使剑不再跟随目标移动
        isReturning = true; // 设置为正在返回状态

    }
    private void Update()
    {
        if (canRotate)
        {
            // 在预制体中，将剑的初始朝向设置为水平向右
            transform.right = rb.linearVelocity; // 让剑的朝向与速度方向一致
        }

        if (isReturning)
        {
            transform.position=Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime); // 将剑移动到玩家位置，平滑移动

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword(); // 如果剑与玩家的距离小于2，则清除剑，表示剑已回到玩家手中并销毁
                //玩家与剑合并，哈哈，人剑合一，可以再次使用剑技能了
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return; // 如果正在返回玩家手中，则不处理碰撞

        anim.SetBool("Rotation", false);
        canRotate = false;
        cd.enabled = false; // 禁用碰撞体，防止多次触发

        rb.bodyType = RigidbodyType2D.Kinematic; // 使用 bodyType 替代已过时的 isKinematic, 使剑 不受物理引擎影响（如重力、碰撞力），但仍可通过代码控制移动。
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有约束，防止物理交互

        transform.parent = collision.transform; // 将剑设置为碰撞物体的子物体, 使剑 跟随目标移动（例如剑插在敌人身上，敌人移动时剑也会移动）。
    }
}
