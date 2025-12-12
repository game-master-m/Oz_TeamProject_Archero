
using System.Collections;
using UnityEngine;
public class StopState : PlayerState
{
    public StopState(PlayerController player, IState parent = null) : base(player, parent) { }
    public override void Enter()//Stop 애니메이션일때 한번 실행
    {
        //유니티 에디터에서만 로그찍기
        Utils.Log("Stop Enter");

        //애니메이션 전환( CrossFade(clip name, 전환시간) , Play(clip name) )
        mPlayer.Anim.CrossFade(AnimHash.idle, 0.1f);

        if (mPlayer.CheckEnemyInRangeCo == null) //방어코드
        {
            //StopState는 Monobehabior가 아니기 때문에 PlayerController(mPlayer)에서 StartCoroutine 실행
            //적 탐지 코루틴 실행(0.1초 마다 탐지)
            mPlayer.CheckEnemyInRangeCo = mPlayer.StartCoroutine(mPlayer.CheckEnemyInAttackRange());
        }
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        //현재 상태를 빠져나가기 전 한번 호출.
    }


}
