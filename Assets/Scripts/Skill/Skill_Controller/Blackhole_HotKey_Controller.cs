using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy; // 用于存储敌人位置的Transform
    private Blackhole_Skill_Controller blackHole; // 引用黑洞技能控制器

    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myHotKey = _myNewHotKey;

        myEnemy = _myEnemy;
        blackHole = _myBlackHole;

        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            // Debug.Log("HOT KEY IS " + myHotKey);
            blackHole.AddEnemyToList(myEnemy); // 将敌人添加到黑洞的目标列表中

            myText.color = Color.clear; // 隐藏热键文本
            sr.color = Color.clear; // 隐藏热键图标
        }
    }
}
