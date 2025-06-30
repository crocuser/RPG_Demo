using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer; // ��ʱ��
    protected bool triggerCalled; // ��������־

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        // ���캯��
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    // ���룬�˳���������ֻ���ж��Ƿ����������������ֶ����������ת����ϵ
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        // ���������ͱ���ֵ�������������������ã���࣬�����ԣ��ɶ��ԣ��ǳ�����!!!
        rb = player.rb;

        triggerCalled = false; // ����״̬�󣬱��δ����
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // ���¶�ʱ����ÿ��״̬�����õ���ʱ��

        // ɾ��ĳ�д����ݼ���ctrl+L
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.linearVelocity.y); // ������������y���ٶȻ�ʵʱ����
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
