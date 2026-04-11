using UnityEngine;

[CreateAssetMenu(fileName = "Freeze", menuName = "Scriptable Objects/Item effect/Freeze")]

public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration; // 冰冻持续时间

    public override void ExecuteEffect(Transform _transform)
    {
        if (!Inventory.instance.CanUseArmor()) // 只有当玩家可以使用盔甲时才会执行这个效果
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.StartFreezeTimeFor(duration);
        }
    }
}
