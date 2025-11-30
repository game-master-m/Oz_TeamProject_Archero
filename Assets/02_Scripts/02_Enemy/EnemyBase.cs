using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : LivingEntity
{

    protected NavMeshAgent mAgent;

    protected virtual void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        // NavMeshAgent 세팅 (속도, 회전 등)
        mAgent.updateRotation = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // 부모의 체력 초기화 실행

        // 적이 다시 살아날 때(풀링) 필요한 초기화
    }

    public override void Die()
    {
        base.Die();

        // 1. 움직임 멈춤
        mAgent.isStopped = true;

        // 2. 콜라이더 끄기 (시체에 공격 안 막히게)
        GetComponent<Collider>().enabled = false;

        // 3. RoomManager에게 알리기 (이벤트나 매니저 호출)
        // Manager.Room.OnEnemyKilled(this);

        // 4. 애니메이션 재생 후 풀로 반환 
    }
}
