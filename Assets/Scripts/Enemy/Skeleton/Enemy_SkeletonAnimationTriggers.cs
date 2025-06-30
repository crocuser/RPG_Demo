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
        // �ռ�������Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
            if (hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damage(); // �������
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow(); // �򿪷������ε���ʱ��ͼ����ʾ
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow(); // �رշ������ε���ʱ��ͼ����ʾ

}
