using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab; // 克隆技能的预制体
    [SerializeField] private float cloneDuration; // 克隆体持续时间
    [Space]
    [SerializeField] private bool canAttack; // 是否拥有该技能

    public void CreateClone(Transform _clonePosition, float _dashDir, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _dashDir, _offset); // 设置克隆体的位置和其他属性
    }
}
