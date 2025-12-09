
using UnityEngine;
public class StopState : PlayerState
{

    private float mTimer = 0.0f;
    public StopState(PlayerController player, IState parent = null) : base(player, parent)
    {
    }
    public override void Enter()
    {
        //유니티 에디터에서만 로그찍기
        Utils.Log("Stop Enter");
        //애니메이션 전환( CrossFade(clip name, 전환시간) , Play(clip name) )
        mPlayer.Anim.CrossFade(AnimHash.idle, 0.1f);

        //테스트 쏘기
        if (!mPlayer.Attack.IsAutoTurret)
        {
            mPlayer.Attack.MakeProjectile();
        }
    }
    public override void Update()
    {


    }
    public override void FixedUpdate() { }
    public override void Exit() { }



}
