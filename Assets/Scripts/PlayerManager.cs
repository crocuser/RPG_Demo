using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //单例模式
    public static PlayerManager instance; // 重命名快捷键 ctrl+r;ctrl+r
    public Player player;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject); // Destroy(instance) 是单例模式的 防御性编程，确保全局唯一性。
        else
            instance = this;
    }
}
