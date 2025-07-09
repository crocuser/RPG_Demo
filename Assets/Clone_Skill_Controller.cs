using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float colorLosingSpeed; // 克隆体颜色消失速度

    private float cloneTimer; // 克隆体计时器

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            // 1,1,1 是保持原本颜色的RGB值，a是透明度
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed)); // 逐渐减少克隆体的透明度
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration)
    {
        transform.position = _newTransform.position; // 设置克隆体的位置
        cloneTimer = _cloneDuration; // 重置克隆体计时器
    }
}
