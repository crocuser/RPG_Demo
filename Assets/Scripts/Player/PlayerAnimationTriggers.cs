using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //�����¼�ֻ�ܵ��ñ������ϵĺ�����
    private Player player => GetComponentInParent<Player>(); // �ýű�������animator�����ĸ�����player

    private void AnimationTrigger()
    {
        player.AnimationTrigger(); // ״̬ӵ�н�����������player�����װ�������⿪�ţ����ڵ���
    }

    private void AttackTrigger()
    {
        // �ռ�������Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
    }
}
