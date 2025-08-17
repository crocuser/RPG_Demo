using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackholeState); // 按下R键，转换为黑洞状态

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword()) // 如果按下鼠标右键且没有持剑，则转换为瞄准剑状态
            stateMachine.ChangeState(player.aimSwordState); // 因为在投掷剑时，才将玩家和剑进行分离，将给玩家赋予一个新的剑

        if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttackState); // 按下Q键，转换为反击状态

        // 按下鼠标左键攻击
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);

        // 如果不在地面，则转换为空气状态
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        //在地面状态中，按下空格，则转换为跳跃状态
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true; // 如果玩家没有持剑，则返回true
        }
        else
        {
            player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword(); // 如果玩家持有剑，则将剑返回玩家手中
            return false; // 如果玩家持有剑，则返回false
        }
    }
}
