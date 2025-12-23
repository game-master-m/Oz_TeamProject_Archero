using UnityEngine;
using UnityEngine.AI;

public class BlackBoard
{
    //실시간 참조 데이터
    public Vector3 NextDestination { get; set; }
    public Transform Target { get; set; }
    public float DistToTarget { get; set; }
    public float CurrentWaitTime { get; set; }
    public Vector3 LastKnownPos { get; set; }

    //AI 상태 데이터
    public float HPPercent { get; set; } = 1.0f;
    public bool IsAngry { get; set; } = false;

    //쿨타임 관리(Dictionary로 바꿔야 할 듯
    public float AttackCoolDown { get; set; }

    //프리팹 투사체 접근용
    public SmallFireBall SmallFireBallPrefab { get; set; }
    public HomingFireBall HomingFireBallPrefab { get; set; }
    public BigFireBall BigFireBallPrefab { get; set; }

    //프리팹 이펙트 접근용
    public EffectBase CurrentEffect { get; set; }
    public FireTrail FireTrailPrefab { get; set; }

    //
}
