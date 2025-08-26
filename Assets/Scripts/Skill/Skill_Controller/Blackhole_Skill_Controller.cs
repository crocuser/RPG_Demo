using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab; // 热键预制体
    [SerializeField] private List<KeyCode> keyCodeList; // 热键列表

    private float maxSize; // 黑洞的最大尺寸
    private float growSpeed; // 黑洞的增长速度
    private float shrinkSpeed; // 黑洞的缩小速度
    private float blackholeTimer; // 黑洞持续时间计时器

    private bool canGrow = true; // 是否可以增长
    private bool canShrink; // 是否可以缩小
    private bool canCreateHotKey = true; // 是否可以创建热键
    private bool cloneAttackReleased; // 是否可以攻击
    private bool playerCanDisapear = true; // 玩家是否可以消失

    private int amountAttack; // 克隆体的攻击次数，如会随着技能等级提升而增加
    private float cloneAttackCooldown; // 克隆体攻击的冷却时间
    private float cloneAttackTimer; // 克隆体攻击计时器

    private List<Transform> targets = new List<Transform>(); // 目标列表，私有化引用变量注意初始化
    private List<GameObject> createdHotKey = new List<GameObject>(); // 已创建的热键游戏对象列表

    public bool playerCanExitState { get; private set; } // 玩家是否可以退出状态

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountAttack, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountAttack = _amountAttack;
        cloneAttackCooldown = _cloneAttackCooldown;

        // It is QTE system enabled.
        blackholeTimer = _blackholeDuration;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            // 为了只做一次检查，将黑洞时间设置成无穷大
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbillity(); // 如果黑洞持续时间结束且没有目标，则直接结束技能
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
                PlayerManager.instance.player.MakeTransparent(false);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            FinishBlackholeAbillity(); // 如果没有目标，则直接结束技能
            return; // 如果没有目标，则不执行克隆攻击
        }

        DestroyHotKeys(); // 销毁所有创建的热键
        cloneAttackReleased = true;
        canCreateHotKey = false; // 禁止创建新的热键

        if (playerCanDisapear)
        {
            playerCanDisapear = false; // 禁止玩家消失
            PlayerManager.instance.player.MakeTransparent(true); // 使玩家透明，表示技能正在释放
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountAttack > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            float offsetX = 0f;
            if (Random.Range(0, 100) > 50)
            {
                offsetX = 2f; // 偏移量向右
            }
            else
            {
                offsetX = -2f; // 偏移量向左
            }

            int randomIndex = Random.Range(0, targets.Count); // 随机选择一个目标

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(offsetX, 0)); // 创建克隆体攻击目标

            amountAttack--; // 减少攻击次数

            if (amountAttack <= 0)
            {
                Invoke("FinishBlackholeAbillity", .35f); // 调用 FinishBlackholeAbillity 方法，延迟0.5秒，等待克隆攻击结束
            }
        }
    }

    private void FinishBlackholeAbillity()
    {
        cloneAttackReleased = false; // 如果攻击次数用完，则不能再攻击
        DestroyHotKeys(); // 销毁所有创建的热键，对于黑洞技能来说，热键只在技能持续期间有效，就是倒计时结束了，玩家没按热键，就销毁热键

        playerCanExitState = true; // 设置玩家可以退出状态
        canShrink = true; // 可以缩小黑洞
    }

    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]); // 销毁创建的热键
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false); // 解除敌人的冻结状态
        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot keys in a key code list!");
            return;
        }

        if (!canCreateHotKey)
            return; // 如果克隆攻击已释放，则不再创建新的热键

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey); // 将新创建的热键添加到列表中

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)]; // 随机选择一个热键
        keyCodeList.Remove(choosenKey); // 从列表中移除已使用的热键

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this); // 设置热键
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform); // 添加敌人到目标列表
}
