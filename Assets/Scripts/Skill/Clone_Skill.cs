using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab; // 克隆技能的预制体
    [SerializeField] private float cloneDuration; // 克隆体持续时间
    [Space]
    [SerializeField] private bool canAttack; // 是否拥有该技能

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;

    public void CreateClone(Transform _clonePosition, float _dashDir, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _dashDir, _offset, FindClosestEnemy(newClone.transform)); // 设置克隆体的位置和其他属性
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, player.dashDir, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, player.dashDir, Vector3.zero);
    }
}
