using System.Collections;
using UnityEngine;

public class EnityFX : MonoBehaviour
{
    private SpriteRenderer sr; // 渲染器

    [Header("Flash FX")]
    [SerializeField] private float flashDuration; // 闪烁持续时间
    [SerializeField] private Material hitMat; //效果就是闪烁一下
    private Material originalMat; // 原始材质

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // 获取子物体的SpriteRenderer组件
        originalMat = sr.material; // 获取保存原始材质
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat; // 设置为闪烁材质

        yield return new WaitForSeconds(flashDuration); // 等待0.2秒

        sr.material = originalMat; // 恢复原始材质
    }

    private void RedColorBlink()
    {
        // 红白闪烁效果
        if (sr.color != Color.white)
            sr.color = Color.white; // 确保颜色是白色的
        else
            sr.color = Color.red; // 如果颜色是白色的，设置为红色
    }

    private void CancelRedBlink()
    {
        // 停止红白闪烁
        CancelInvoke("RedColorBlink"); // 停止InvokeRepeating调用
        sr.color = Color.white; // 恢复为白色
    }
}
