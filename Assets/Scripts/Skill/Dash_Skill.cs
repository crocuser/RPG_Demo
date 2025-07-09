using UnityEngine;

public class Dash_Skill : Skill
{
    public override void UseSkill()
    {
        base.UseSkill();

        // 技能的内容：好像是冲刺后面创建一个克隆
        Debug.Log("Create clone behind");
    }
}
