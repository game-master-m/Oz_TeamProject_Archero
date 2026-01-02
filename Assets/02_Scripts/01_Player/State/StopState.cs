
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

        //AutoTurret을 먹으면 아래 코루틴이 실행 될 일이 없고, ThrowState로도 못 넘어감
        if (!mPlayer.Attack.IsAutoTurret)
        {
            if (mPlayer.CheckEnemyInRangeCo == null) //방어코드
            {
                //StopState는 Monobehabior가 아니기 때문에 PlayerController(mPlayer)에서 StartCoroutine 실행
                //적 탐지 코루틴 실행(0.1초 마다 탐지)
                //종료는 ThrowState Exit()와 MoveState Enter()에서 해줌.
                mPlayer.CheckEnemyInRangeCo = mPlayer.StartCoroutine(mPlayer.CheckEnemyInAttackRange());
            }
        }
        //else
        //{
        //    mPlayer.StopCoroutine(mPlayer.CheckEnemyInRangeCo);
        //    mPlayer.CheckEnemyInRangeCo = null;
        //}
    }
    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        //현재 상태를 빠져나가기 전 한번 호출.
    }


}
