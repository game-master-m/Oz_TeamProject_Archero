

using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(PlayerController player, IState parent = null) : base(player, parent)
    {
    }

    public override void Enter()
    {
        //로그찍는 함수
        Utils.Log("Move Enter");
        mPlayer.Anim.CrossFade(AnimHash.move, 0.1f);
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
