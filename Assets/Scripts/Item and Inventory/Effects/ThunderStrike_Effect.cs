using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Scriptable Objects/Item effect/Thunder strike")]

public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosiition)
    {
        //Time.timeScale = 0.1f;
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosiition.position, Quaternion.identity);

        Destroy(newThunderStrike, 0.5f); // 0.5秒后销毁雷电特效
    }
}
