using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        // 收集攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage(); // 攻击玩家
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow(); // 打开反击击晕敌人时的图像提示
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow(); // 关闭反击击晕敌人时的图像提示

}
