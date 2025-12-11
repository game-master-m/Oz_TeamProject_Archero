using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : EnemyState
{
    public RangeAttackState(EnemyBase enemy, IState parent = null) : base(enemy, parent)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Utils.Log("RangeAttack Enter");
        //mEnemy.Anim.CrossFade(AnimHash.attack, 0.1f); //공격애니메이션을 만들거나 아니면 삭제..?
    }

    public override void Update()
    {
        base.Update();
        //여기에 원거리 공격 상태에서 필요한 로직
        //투사체를 발사한다거나, 공격 타이밍을 조절한다거나 
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        // 공격이 끝났을 때 필요한 정리 작업들
    }
}
