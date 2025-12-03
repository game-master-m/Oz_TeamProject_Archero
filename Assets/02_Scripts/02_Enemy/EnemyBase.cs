using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : LivingEntity
{
    [SerializeField] protected EnemyStatDataSO mStatData;

    protected Animator mAnimator;
    protected NavMeshAgent mAgent;

    //플레이어 추적용 타겟
    protected Transform mTarget;

    public event Action<EnemyBase> onEnemyDie;

    protected virtual void Awake()
    {

        mAnimator = GetComponent<Animator>();
        mAgent = GetComponent<NavMeshAgent>();

        // NavMeshAgent 세팅 (속도, 회전 등)
        mAgent.updateRotation = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // 부모의 체력 초기화 실행

        if (mAgent != null)
        {
            mAgent.enabled = true;
            mAgent.isStopped = false;
        }

        //컬라이더 키기

        // 적이 다시 살아날 때(풀링) 필요한 초기화
    }
    public virtual void InitStats(EnemyStatDataSO data)
    {
        base.Init(data.MaxHP);
        mAgent.speed = data.MoveSpeed;
    }
    public void SetTarget(Transform target)
    {
        mTarget = target;
    }

    protected virtual void Update()
    {
        //테스트용 타겟 추척
        if (mTarget != null)
        {
            mAgent.SetDestination(mTarget.position);
        }
    }

    public override void Die()
    {
        base.Die();
        // 0. 죽음 방송~
        onEnemyDie?.Invoke(this);

        // 1. 움직임 멈춤
        mAgent.isStopped = true;
        mAgent.enabled = false;

        // 2. 콜라이더 끄기 (시체에 공격 안 막히게)
        GetComponent<Collider>().enabled = false;

        // 3. StageManager에게 알리기 (이벤트나 매니저 호출)


        // 4. 애니메이션 재생 후 풀로 반환 
        Managers.Pool.ReturnToPool(this);
    }
}
