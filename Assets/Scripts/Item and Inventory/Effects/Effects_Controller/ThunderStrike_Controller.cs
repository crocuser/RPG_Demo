using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected PlayerStats playerStats;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
        }
            //Debug.Log("是否执行了碰撞");
    }
}
