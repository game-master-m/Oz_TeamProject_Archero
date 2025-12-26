using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SlimeController : EnemyBase
{
    /*
    컨트롤러에서 공격페이즈들(bt로 만든)결정
    여기서 InitTransitions 상태전환(애니메이터 안씀)
    스테이트에선 아무것도안함
    각행동 노드 > 액션노드 > 노드
    */
    SlimeMoveState mMoveState;
    SlimeAttackState mAttackState;
    

    protected override void Awake()
    {
        base.Awake();

        InitTransitions();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void InitTransitions()
    {
        //움직이다 멈추고 쏘기반복?
        //mStateMachine.AddTransition(mMoveState, mAttackState, () =>);
    }
}
