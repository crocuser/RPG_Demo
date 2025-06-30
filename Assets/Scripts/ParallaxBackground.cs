using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam; // 主摄像机对象
    [SerializeField] private float parallaxEffect; // 视差系数（控制移动比例）：0-1

    private float xPosition; // 背景物体的初始X坐标
    private float length;

    private void Start()
    {
        cam = GameObject.Find("Main Camera"); // 查找场景中的主摄像机
        
        xPosition = transform.position.x; // 记录背景物体的初始X位置
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // 计算背景应该移动的距离：摄像机当前X位置 × 视差系数
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        // 背景中心点离相机的位置
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        // 更新背景位置：初始X位置 + 计算出的偏移量，Y轴保持不变
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        // 无尽跑道的感觉！
        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
    }
}