using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed; // 克隆体颜色消失速度
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f; // 攻击检测半径
    private Transform closestEnemy; // 最近的敌人
    private bool facingRight; // 面向方向
    private int facingDir = 1;

    private bool canDuplicateClone; // 是否可以复制克隆体
    private float chanceToDuplicate; // 复制克隆体的概率

    private float cloneTimer; // 克隆体计时器

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            // 1,1,1 是保持原本颜色的RGB值，a是透明度
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed)); // 逐渐减少克隆体的透明度

            if (sr.color.a <= 0)
                Destroy(gameObject); // 如果透明度小于等于0，则销毁克隆体
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _facingRight, bool _canDuplicateClone, float _chanceToDuplicate, Player _player)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4)); // 随机设置攻击动画编号
        }
        transform.position = _newTransform.position + _offset; // 设置克隆体的位置
        cloneTimer = _cloneDuration; // 重置克隆体计时器

        closestEnemy = _closestEnemy; // 设置最近的敌人
        facingRight = _facingRight;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;

        player = _player;
        FaceClosestTarget(); // 面向最近的敌人
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f; // 设置克隆体计时器为负值，触发消失
    }

    private void AttackTrigger()
    {
        // 收集攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().DamageImpact();//在enemy脚本里处理
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if (canDuplicateClone)
                {
                    // 新的技能模式，克隆体有99%的概率生成一个新的克隆体
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * facingDir, 0)); // 在敌人旁边生成一个新的克隆体
                    }
                }
            }

        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
        else if (closestEnemy == null && !facingRight)
        {
            //Debug.Log("没有发现敌人");
            transform.Rotate(0, 180, 0); // 如果是向左冲刺，则旋转克隆体，默认是右侧
        }
    }
}
