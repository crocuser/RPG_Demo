using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Scriptable Objects/Item effect/Heal")]

public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)] // 0% 到 100%
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosiition)
    {
        // 获取玩家的状态信息，增加玩家的血量
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        playerStats.IncreaseHealthBy(healAmount);
    }
}
