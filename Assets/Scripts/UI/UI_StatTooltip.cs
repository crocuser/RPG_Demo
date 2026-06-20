using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    private bool isShowing = false; // 标记当前是否显示

    void Update()
    {
        // 如果提示框正在显示，每帧更新位置到鼠标
        if (isShowing)
        {
            UpdatePositionToMouse();
        }
    }

    public void ShowStatTooltip(string _text)
    {
        isShowing = true;

        description.text = _text;
        // Show the tooltip with the description
        gameObject.SetActive(true);

        // 调用通用方法
        UpdatePositionToMouse();
    }

    public void HideStatTooltip()
    {
        isShowing = false;

        description.text = "";
        // Hide the tooltip
        gameObject.SetActive(false);
    }
    public void UpdatePositionToMouse()
    {
        RectTransform tooltipRect = GetComponent<RectTransform>();

        // 获取鼠标位置
        Vector2 mouseScreenPos = Input.mousePosition;

        // 获取父 Canvas
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        Vector2 localPos;
        // 把屏幕鼠标像素坐标 → 转换成 Canvas 内部的局部锚点坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(),
            mouseScreenPos,
            parentCanvas.worldCamera, // Canvas 是 ScreenSpaceCamera 时传相机，Overlay 传 null
            out localPos
        );

        //// 默认中心对齐
        //tooltipRect.anchoredPosition = localPos;

        //如果想左上角对齐鼠标，取消下面注释
        Vector2 pivotOffset = new Vector2(tooltipRect.rect.width * 0.5f + 20f, -tooltipRect.rect.height * 0.5f - 20f);
        tooltipRect.anchoredPosition = localPos + pivotOffset;
    }
}
