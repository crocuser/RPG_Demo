using UnityEngine;
using UnityEngine.UI;

public class UI_Pages : MonoBehaviour
{
    public GameObject[] pages;
    public Button[] buttons;

    public Color normalColor;
    public Color selectedColor;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void ShowPage(int index)
    {
        // 显示选中的页面，隐藏其他页面
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
        // 更新按钮状态
        UpdateButtonState(index);
    }

    void UpdateButtonState(int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colors = buttons[i].colors;

            colors.normalColor = (i == selectedIndex) ? selectedColor : normalColor;

            buttons[i].colors = colors;
        }
    }
}
