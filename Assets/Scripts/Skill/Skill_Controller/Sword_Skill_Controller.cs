using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning = false; // 是否正在返回玩家手中

    private float freezeTimeDuration; // 冻结时间持续时间
    private float returnSpeed; // 返回玩家手中的速度

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount; // 穿透攻击的伤害量


    [Header("Bounce info")]
    private float bounceSpeed; // 反弹攻击的速度
    private bool isBouncing; // 是否可以反弹攻击
    private int bounceAmount; // 反弹次数
    private List<Transform> enemyTarget; // 目标敌人列表，用于存储反弹攻击的目标
    private int targetIndex; // 当前目标索引

    [Header("Spin info")]
    private float maxTravelDistance; // 最大飞行距离
    private float spinaDuration; // 旋转持续时间
    private float spinTimer; // 旋转计时器
    private bool wasStopped; // 旋转停止标志
    private bool isSpinning; // 是否正在旋转

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection; // 旋转方向

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject); // 销毁当前游戏对象，当剑飞出太远时，销毁剑
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player; // 设置玩家引用，便于后续操作
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed; // 设置返回速度

        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);// 如果没有穿透攻击，则开始旋转动画

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1); // 限制旋转方向为-1到1之间，避免过大或过小的值导致异常行为

        Invoke("DestroyMe", 7);
    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing; // 设置是否可以反弹攻击
        bounceAmount = _bounceAmount; // 设置反弹次数
        bounceSpeed = _bounceSpeed; // 设置反弹攻击的速度

        enemyTarget = new List<Transform>(); // 初始化目标敌人列表，如果是私有的，需要初始化列表
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinaDuration = _spinDuration;
        hitCooldown = _hitCooldown;
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
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime); // 将剑移动到玩家位置，平滑移动

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword(); // 如果剑与玩家的距离小于2，则清除剑，表示剑已回到玩家手中并销毁
                //玩家与剑合并，哈哈，人剑合一，可以再次使用剑技能了
            }
        }

        BounceLogic();

        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime); // 旋转时，剑沿着水平方向移动

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown; // 攻击频率冷却
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1); // 在一定范围内检测敌人

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>()); // 对目标敌人造成伤害
                    }
                }
                // Debug.Log("Spinning... " + spinTimer); // 打印旋转计时器信息
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // 冻结位置，停止剑的移动
        spinTimer = spinaDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>()); // 对目标敌人造成伤害

                targetIndex++; // 移动到下一个目标

                bounceAmount--; // 减少反弹次数

                if (bounceAmount <= 0)
                {
                    isBouncing = false; // 如果反弹次数用完，则停止反弹攻击
                    isReturning = true; // 开始返回玩家手中
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0; // 如果到达最后一个目标，则重置索引
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return; // 如果正在返回玩家手中，则不处理碰撞


        if (collision.GetComponent<Enemy>() != null) // 如果碰撞到敌人，则对敌人造成伤害
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }

        // 如果碰撞到敌人，则收集敌人位置并填充目标列表
        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10); // 在一定范围内检测敌人

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform); // 将检测到的敌人添加到目标列表中

                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--; // 如果是穿透攻击，则减少穿透次数
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning(); // 如果碰撞物体，则固定位置
            return;
        }

        canRotate = false;
        cd.enabled = false; // 禁用碰撞体，防止多次触发

        rb.bodyType = RigidbodyType2D.Kinematic; // 使用 bodyType 替代已过时的 isKinematic, 使剑 不受物理引擎影响（如重力、碰撞力），但仍可通过代码控制移动。
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有约束，防止物理交互

        if (isBouncing && enemyTarget.Count > 0)
            return; // 如果是反弹攻击，则不执行后续操作

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform; // 将剑设置为碰撞物体的子物体, 使剑 跟随目标移动（例如剑插在敌人身上，敌人移动时剑也会移动）。
    }
}
