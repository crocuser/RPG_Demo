   using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration; // 水晶存在时间
    [SerializeField] private GameObject crystalPrefab; // 水晶预制体
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal; // 是否是克隆体而不是传送

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode; // 是否可以爆炸

    [Header("Moving crystal")]
    [SerializeField] private bool canMove; // 是否可以移动
    [SerializeField] private float moveSpeed; // 水晶移动速度

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks; // 是否可以使用多水晶
    [SerializeField] private int amountOfStacks; // 水晶数量
    [SerializeField] private float multiStackCooldown; // 多水晶叠冷却时间
    [SerializeField] private float useTimeWindow; // 使用时间窗口
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>(); // 相当于存放了水晶的现有预制体

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return; // 如果可以使用多水晶，则直接返回

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMove)
                return; // 如果水晶可以移动，则不允许再次使用技能的其他模式

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position; // 将玩家位置设置为水晶位置
            currentCrystal.transform.position = playerPos; // 将水晶位置设置为玩家位置

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero); // 创建克隆体
                Destroy(currentCrystal); // 销毁水晶
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal(); // 结束当前水晶
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(currentCrystal.transform)); // 设置水晶的持续时间

    }

    public void CurrentCrystalChooseRandomTarget() =>
        currentCrystal?.GetComponent<Crystal_Skill_Controller>()?.ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbiity", useTimeWindow); // 如果水晶数量等于堆叠数量，意味着使用了第一个水晶

                cooldown = 0; // 重置冷却时间
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1]; // 只是获取当前列表中的最后一个水晶预制体，那为什么不存成一个int索引呢？[@crocuser 疑惑]，哦明白了，是因为list本身存的只是对象引用而已
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMove, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown; // 重置多水晶冷却时间
                    RefilCrystal(); // 重新填充水晶
                }
               
                return true;
            }
        }

        return false;
    }
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count; // 计算需要添加的水晶数量

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbiity()
    {
        if (cooldownTimer > 0)
            return; // 冷却时间在重置中，直接返回

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }

}
