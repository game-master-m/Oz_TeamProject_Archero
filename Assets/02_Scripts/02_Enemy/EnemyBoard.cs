using UnityEngine;

public class EnemyBoard
{
    //실시간 참조 데이터
    public Transform Target { get; set; }
    public float DistToTarget { get; set; }

    //AI 상태 데이터
    public float HPPercent { get; set; } = 1.0f;
    public bool IsAngry { get; set; } = false;
    public Vector3 LastKnownPos { get; set; }

    //쿨타임 관리
    public float AttackCoolDown { get; set; }
}
