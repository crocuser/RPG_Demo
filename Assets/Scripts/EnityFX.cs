using System.Collections;
using UnityEngine;

public class EnityFX : MonoBehaviour
{
    private SpriteRenderer sr; // 渲染器

    [Header("Flash FX")]
    [SerializeField] private float flashDuration; // 闪烁持续时间
    [SerializeField] private Material hitMat; //效果就是闪烁一下
    private Material originalMat; // 原始材质

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor; // 点燃颜色数组，用于渐变效果
    [SerializeField] private Color[] chillColor; // 冰冻颜色
    [SerializeField] private Color[] shockColor; // 电击颜色数组，用于渐变效果

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // 获取子物体的SpriteRenderer组件
        originalMat = sr.material; // 获取保存原始材质
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear; // 设置透明
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat; // 设置为闪烁材质
        Color currentColor = sr.color; // 保存当前颜色
        sr.color = Color.white; // 设置为白色以增强闪烁效果，不会被燃烧颜色覆盖

        yield return new WaitForSeconds(flashDuration); // 等待0.2秒

        sr.color = currentColor; // 恢复原始颜色
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

    private void CancelColorChange()
    {
        // 停止红白闪烁
        CancelInvoke(); // 停止InvokeRepeating调用
        sr.color = Color.white; // 恢复为白色
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f); // 每0.3秒切换一次颜色
        Invoke("CancelColorChange", _seconds); // 经过_seconds秒后取消颜色变化
    }
    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFx", 0, .3f); // 每0.3秒切换一次颜色
        Invoke("CancelColorChange", _seconds); // 经过_seconds秒后取消颜色变化
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f); // 每0.3秒切换一次颜色
        Invoke("CancelColorChange", _seconds); // 经过_seconds秒后取消颜色变化
    }
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
}