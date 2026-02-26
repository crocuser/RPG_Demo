using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire effect", menuName = "Scriptable Objects/Item effect/Ice And Fire")]

public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respondPosiition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2; // 判断是否为第三次攻击

        if (thirdAttack)
        { 
            // 创建冰火特效，并设置其位置和旋转（玩家的朝向）
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosiition.position, player.transform.rotation);
            // 设置冰火特效的速度
            newIceAndFire.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newIceAndFire, 6);
        }
    }
}
