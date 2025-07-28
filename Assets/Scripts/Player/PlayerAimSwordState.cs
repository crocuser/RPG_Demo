using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true); // 激活瞄准点
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f); // 退出状态时，玩家变为忙碌状态，持续0.2秒
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity(); // 设置玩家速度为零，防止移动

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState); // 松开鼠标右键，转换为待机状态

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取鼠标位置

        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
            player.Flip(); // 如果鼠标在玩家左边，且玩家面向右侧，则翻转玩家
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1)
            player.Flip(); // 如果鼠标在玩家右边，且玩家面向左侧，则翻转玩家
    }
}
