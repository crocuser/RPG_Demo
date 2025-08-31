using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown; // 技能冷却时间
    protected float cooldownTimer; // 技能冷却计时器

    protected Player player;
    protected LayerMask enemyLayer;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    protected virtual void Update() // 嗯，注意Update首字母大写，以及Unity自带的方法是蓝色标识
    {
        cooldownTimer -= Time.deltaTime; // 减少冷却计时器
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill(); // 使用技能
            cooldownTimer = cooldown; // 重置冷却计时器
            return true; // 可以使用技能
        }

        // Debug.Log("Skill is on cooldown");
        return false; // 技能冷却中，不能使用技能
    }

    public virtual void UseSkill()
    {
        // do some skill spesific things
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25, enemyLayer); // 检测范围内的所有碰撞体

        float closestDistance = Mathf.Infinity; // 初始化最近距离为无穷大
        Transform closestEnemy = null; // 初始化最近的敌人为null

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null) // 如果碰撞体是敌人
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position); // 计算与克隆体的距离
                if (distanceToEnemy <= closestDistance) // 如果距离小于当前最近距离
                {
                    closestDistance = distanceToEnemy; // 更新最近距离
                    closestEnemy = hit.transform; // 更新最近的敌人
                }
                // Debug.Log("Found enemy: " + hit.name + " at distance: " + distanceToEnemy);
            }
        }
        return closestEnemy; // 返回最近的敌人
    }
}
