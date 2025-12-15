

using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(PlayerController player, IState parent = null) : base(player, parent) { }

    public override void Enter()
    {
        //로그찍는 함수
        Utils.Log("Move Enter");

        mPlayer.Anim.CrossFade(AnimHash.move, 0.1f);

        //StopState에서 시작 한 에너미 체크 및 회전 코루틴 꺼줌
        if (mPlayer.RotateToTargetCo != null)
        {
            mPlayer.StopCoroutine(mPlayer.RotateToTargetCo);
            mPlayer.RotateToTargetCo = null;
        }
        if (mPlayer.CheckEnemyInRangeCo != null)
        {
            mPlayer.StopCoroutine(mPlayer.CheckEnemyInRangeCo);
            mPlayer.CheckEnemyInRangeCo = null;
        }
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit() { }
}
