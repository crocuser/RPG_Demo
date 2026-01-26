using System;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject inventorySlotPanel;
    [SerializeField] private GameObject stashSlotPanel;
    [SerializeField] private GameObject equipmentSlotPanel;
    [SerializeField] private GameObject CraftSlotPanel;

    private bool isPause = true;

    private float originalTimeScale = 1f;
    private float originalFixedDeltaTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        // 全局快捷键
        HandleGlobalInput();
    }

    void HandleGlobalInput()
    {
        // ESC打开/关闭暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel(CraftSlotPanel);
            //CraftSlotPanel.SetActive(!CraftSlotPanel.activeInHierarchy);
            isPause = !isPause;
            if (isPause)
            {
                Debug.Log("游戏已暂停");
                PauseGame();
            }
            else
            {
                Debug.Log("游戏已恢复");
                ResumeGame();
            }
        }

        // Tab打开/关闭背包（如果没有其他面板打开）
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePanel(inventorySlotPanel); // 性能更好
            TogglePanel(stashSlotPanel);
            TogglePanel(equipmentSlotPanel);

            //inventorySlotPanel.SetActive(!inventorySlotPanel.activeInHierarchy);
            //stashSlotPanel.SetActive(!stashSlotPanel.activeInHierarchy);
            //equipmentSlotPanel.SetActive(!equipmentSlotPanel.activeInHierarchy);
        }
    }
    void TogglePanel(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>(); // UI的群组控制器
        if (cg.alpha > 0.5f) // 如果当前显示
        {
            // 隐藏：看不见也点不到
            cg.alpha = 0;
            cg.blocksRaycasts = false; // 可以鼠标穿透它
        }
        else // 如果当前隐藏
        {
            // 显示：看得见也能点
            cg.alpha = 1;
            cg.blocksRaycasts = true;
        }
    }

    public void PauseGame()
    {
        originalTimeScale = Time.timeScale;

        // 暂停游戏
        Time.timeScale = 0;

        // 重要！也需要暂停物理更新
        Time.fixedDeltaTime = 0;

    }

    public void ResumeGame()
    {
        // 恢复游戏
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;

    }

}
