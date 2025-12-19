
using UnityEngine;
public class DeathState : PlayerState
{
    public DeathState(PlayerController player, IState parent = null) : base(player, parent) { }

    private float mDisappearDuration = 0.5f;
    public override void Enter()
    {
        Utils.Log("플레이어 다이!!!~~!!");
        Time.timeScale = 0.5f;
        mPlayer.CharacterController.enabled = false;
    }
    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
        mElapsedTimeBase += Time.fixedDeltaTime;
        if (mElapsedTimeBase > mDisappearDuration)
        {
            mElapsedTimeBase = 0f;
            //연출추가
        }
    }
    public override void Exit()
    {

    }


}
