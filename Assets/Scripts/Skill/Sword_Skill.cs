using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab; // 剑的预制体
    [SerializeField] private Vector2 launchForce; // 剑的发射方向
    [SerializeField] private float swordGravity; // 剑的重力

    private Vector2 finalDir; // 最终的发射方向

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots; // 瞄准点数量
    [SerializeField] private float spaceBetweenDots; // 瞄准点之间的间距
    [SerializeField] private GameObject dotPrefab; // 瞄准点预制体
    [SerializeField] private Transform dotsParent; // 瞄准点的父物体

    private GameObject[] dots; // 瞄准点数组

    protected override void Start()
    {
        base.Start();
        GenerateDots(); // 生成瞄准点
    }
    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().x * launchForce.x, AimDirection().y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots); // 更新每个瞄准点的位置
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(finalDir, swordGravity, player); // 设置剑的发射方向和重力

        player.AssignNewSword(newSword); // 将新剑分配给玩家

        DotsActive(false); // 剑已发射，禁用瞄准点
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position; // 获取玩家位置
        Vector2 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取鼠标位置
        Vector2 direction = (mousePositon - playerPosition).normalized; // 计算方向向量并归一化

        // 用来生成和设置点，便于瞄准目标
        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive); // 设置每个瞄准点的激活状态
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];    
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(AimDirection().x * launchForce.x, AimDirection().y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
