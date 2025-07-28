using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed; // 克隆体颜色消失速度
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f; // 攻击检测半径
    private Transform closestEnemy; // 最近的敌人

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

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack, float _dashDir)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4)); // 随机设置攻击动画编号
        }
        transform.position = _newTransform.position; // 设置克隆体的位置
        cloneTimer = _cloneDuration; // 重置克隆体计时器

        FaceClosestTarget(_dashDir); // 面向最近的敌人
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
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
    }

    private void FaceClosestTarget(float _dashDir)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackCheckRadius * 3); // 检测范围内的所有碰撞体

        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // 如果碰撞体是敌人
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position); // 计算与克隆体的距离
                if (distanceToEnemy < closestDistance) // 如果距离小于当前最近距离
                {
                    closestDistance = distanceToEnemy; // 更新最近距离
                    closestEnemy = hit.transform; // 更新最近的敌人
                }
            }
        }

        if (closestEnemy != null)
        {
            //Debug.Log("Closest enemy found: " + closestEnemy.name);
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
        else if (_dashDir < 0)
        {
            //Debug.Log("没有发现敌人");
            transform.Rotate(0, 180, 0); // 如果是向左冲刺，则旋转克隆体，默认是右侧
        }
    }
}
