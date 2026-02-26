using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/Item effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _enemyPosiition)
    {
        Debug.Log("Executing base item effect.");
    }
}
