using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats; // 目标属性
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered; // 是否已触发命中效果

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Update()
    {
        if (!targetStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime); // 向目标移动
        transform.right = transform.position - targetStats.transform.position; // 面向目标

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f); // 调整动画位置，让闪电不要戳到地里
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke(nameof(DamageAndSelfDestroy), .2f); // 延迟伤害和销毁，给动画时间播放
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true); // 应用电击状态
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f); // 动画播放完后销毁
    }
}
