using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Objects/Item effect/Buff")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private statType buffType; // 增加属性的类型
    [SerializeField] private int buffAmount; // 增加的属性值
    [SerializeField] private float buffDuration; // 增加属性的持续时间

    public override void ExecuteEffect(Transform _enemyPosiition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }


}
