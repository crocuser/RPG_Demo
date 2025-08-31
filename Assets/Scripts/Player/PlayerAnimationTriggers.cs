using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //动画事件只能调用本物体上的函数。
    private Player player => GetComponentInParent<Player>(); // 该脚本作用与animator，它的父亲是player

    private void AnimationTrigger()
    {
        player.AnimationTrigger(); // 状态拥有结束触发器，player将其封装，并对外开放，便于调用
    }

    private void AttackTrigger()
    {
        // 收集攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
                hit.GetComponent<CharacterStats>().TakeDamage(player.stats.damage);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
