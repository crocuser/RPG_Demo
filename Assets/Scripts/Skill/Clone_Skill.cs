using System.Collections;
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
    [SerializeField] private bool canCreateCloneCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone; // 是否可以复制克隆体
    [SerializeField] private float chanceToDuplicate; // 复制克隆体的概率

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone; // 是否生成水晶而不是克隆体

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(_clonePosition), player.facingRight, canDuplicateClone, chanceToDuplicate, player); // 设置克隆体的位置和其他属性
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(0.5f * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.5f);
        CreateClone(_transform, _offset);
    }
}
