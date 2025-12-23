using System.Collections.Generic;
using UnityEngine;

public class DragonThirdPhaseState : DragonState
{
    private Node mPhase3BT;

    //예측 연발 샷
    private readonly float mMoveSpeed = 14.0f;
    private readonly float mFireInterval = 0.10f;
    private readonly Vector3 mSpawnOffset = new Vector3(0, 1.0f, 0.5f);
    private readonly Vector3 mSpawnOffset2 = new Vector3(0.0f, 3.2f, 2.5f);

    public DragonThirdPhaseState(DragonController dragon, IState parent = null) : base(dragon, parent)
    {
        BuildBT();
    }

    public override void Enter()
    {
        Utils.Log("Dragon Third Phase State 진입!!");
    }
    public override void Update()
    {
        mPhase3BT.Evaluate();
    }
    public override void FixedUpdate() { }
    public override void Exit()
    {
        mPhase3BT.Abort();
    }

    private void BuildBT()
    {
        // ---------------------------------------------------------
        // A. 근접 대응 콤보 (Melee -> Point Blank Spread)
        // ---------------------------------------------------------
        Node meleeCombo = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => Vector3.SqrMagnitude(mDragon.transform.position - mDragon.Target.position) <= mDragon.AttackRange*mDragon.AttackRange),
            new ConditionNode( () => {mDragon.Board.CurrentEffect.ExecuteEffect(); return true; }),
            new RotateToTargetNode(mDragon, mDragon.Board, 30.0f),
            new BasicAttackNode(mDragon, mDragon.Board, 0.26f, 5.0f, 2.65f), // 1페이즈 근접공격
            new SpreadVollyNode(mDragon, mDragon.Board, 8, mMoveSpeed, mFireInterval, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
            new WaitNode(mDragon, 0.5f)
        }, true);

        // ---------------------------------------------------------
        // B. 원거리 추격 콤보 (Dash -> Fan Shot)
        // ---------------------------------------------------------
        Node gapCloserCombo = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => (mDragon.Target.position - mDragon.transform.position).sqrMagnitude > 450.0f),
            new ConditionNode( () => {mDragon.Board.CurrentEffect.ExecuteEffect(); return true; }),
            new SelectorNode(new List<Node>
            {
                new DashAttackNode(mDragon, mDragon.Board, 0.8f, 0.6f, 20.0f, 0.32f, 3.5f),
                new ConditionNode(()=>true)
            }),
            new SpinAttackNode(mDragon, mDragon.AttackCol),
            new RotateToTargetNode(mDragon, mDragon.Board, 12.0f),
            new FanShotNode(mDragon, mDragon.Board, 10, 15.0f, 0.2f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBallPrefab)),
            new WaitNode(mDragon, 0.8f)
        }, true);

        // ---------------------------------------------------------
        // C. 지옥의 탄막 패턴 (Phase 2 패턴의 강화 및 혼합)
        // ---------------------------------------------------------
        Node hellPattern = new RandomSelectorNode(new List<Node>
        {
            // 패턴 1: 화염의 길 예측 샷
            new SequenceNode(new List<Node>{
                new SummonFireTrailNode(mDragon, mDragon.Board, 1.0f, mSpawnOffset2),
                new PredictVolleyNode(mDragon, mDragon.Board, 20, mMoveSpeed, mFireInterval, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
                new ConditionNode( () => {mDragon.Board.CurrentEffect.ExecuteEffect(); return true; }),
            }),
            // 패턴 2: 유도탄 + 확산탄
            new SequenceNode(new List<Node>{
                new FanShotNode(mDragon, mDragon.Board, 5, 12.0f, 0.5f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBallPrefab)),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.SmallFireBallPrefab)),
            }),
            // 패턴 3: 빅 파이어볼 연사
            new SequenceNode(new List<Node>{
                new NormalShotNode(mDragon, mDragon.Board, 10.0f, 1.0f, 1.0f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.BigFireBallPrefab)),
                new NormalShotNode(mDragon, mDragon.Board, 10.0f, 1.0f, 1.0f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.BigFireBallPrefab)),
            }),
            //
            new SequenceNode(new List<Node>{
                new SummonFireTrailNode(mDragon, mDragon.Board, 1.0f, mSpawnOffset2),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBallPrefab)),
                new WaitNode(mDragon, 0.5f),
                new SpreadVollyNode(mDragon, mDragon.Board, 12, mMoveSpeed, 0.05f, mSpawnOffset, () => Managers.Pool.GetFromPool(mDragon.Board.HomingFireBallPrefab)),
                new ConditionNode( () => {mDragon.Board.CurrentEffect.ExecuteEffect(); return true; }),
            }),
        });

        // ---------------------------------------------------------
        // D. 메인 사이클 (Selector 기반 우선순위 결정)
        // ---------------------------------------------------------
        mPhase3BT = new RepeaterNode(
            new SelectorNode(new List<Node>
            {
                // 1순위: 너무 멀면 대쉬로 붙어서 공격
                gapCloserCombo,
                
                // 2순위: 너무 가까우면 근접 공격 후 탄막
                meleeCombo,

                // 3순위: 상시 패턴 및 이동
                new SequenceNode(new List<Node>
                {
                    new RotateToTargetNode(mDragon, mDragon.Board, 15.0f),
                    hellPattern,
                    hellPattern,
                    new WaitNode(mDragon, 0.5f),
                    hellPattern,
                    new SetRandomPatrolDataNode(mDragon, mDragon.Board, 8.0f, 0.5f, 0.8f),
                    new MoveToNextPosNode(mDragon, mDragon.Board),
                    new WaitNode(mDragon, 1.0f)
                }, true)
            })
        );
    }
}
