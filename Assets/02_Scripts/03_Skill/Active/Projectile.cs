using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Projectile : MonoBehaviour
{
    //각각의 프리팹마다 다르게 설정 가능
    [SerializeField] private float mTargetRange = 30.0f;
    [SerializeField] private float mMoveSpeed = 8.0f;
    [SerializeField] private float mLifeTime = 10.0f;
    [SerializeField] private Vector3 mTargetOffset = new Vector3(0, 0.5f, 0);

    //캐싱
    private Rigidbody mRB;
    private CapsuleCollider mCollider;

    // 상태 변수
    private List<IProjectileStrategy> mStrategies = new List<IProjectileStrategy>();
    private float mLifeTimer = 0.0f;

    // 이번 발사체에서 무시할 충돌체 ID 목록
    private HashSet<int> mIgnoreColliderIDs = new HashSet<int>();

    // 프로퍼티
    public int RemainingBounceCount { get; set; } = 0;
    public bool bShouldReturnPool { get; set; } = true;
    public float CurrentDamage { get; private set; }
    public CapsuleCollider Collider => mCollider;

    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mCollider = GetComponent<CapsuleCollider>();
    }
    private void OnEnable()
    {
        mLifeTimer = 0.0f;
        mIgnoreColliderIDs.Clear();
    }


    //MutiShot , SplitShot 등에서 사용, 원본 Projectile의 전략과 데미지 복사
    public void CopyWithOutOnShoot(Projectile giver)
    {
        mStrategies = new List<IProjectileStrategy>(giver.mStrategies);

        CurrentDamage = giver.CurrentDamage;
        RemainingBounceCount = giver.RemainingBounceCount;
        bShouldReturnPool = giver.bShouldReturnPool;
    }
    //특정 전략 제거
    public void RemoveStrategy<T>() where T : IProjectileStrategy
    {
        mStrategies.RemoveAll(strategy => strategy is T);
    }
    public void AddIgnoreTarget(int instanceID)
    {
        if (!mIgnoreColliderIDs.Contains(instanceID))
        {
            mIgnoreColliderIDs.Add(instanceID);
        }
    }
    //PlayerAttack이 갖고 있는 strategies를 주입
    public void Setup(List<IProjectileStrategy> strategies, float damage)
    {

        mStrategies = new List<IProjectileStrategy>(strategies);
        CurrentDamage = damage;

        //각각 발사되는 프로젝타일 변수 초기화
        RemainingBounceCount = 0;
        bShouldReturnPool = true;

        //적이 없으면 발사하지 않음
        if (!LookTarget(mTargetRange))
        {
            return;
        }

        //각각 전략들의 초기화 로직
        foreach (var strategy in mStrategies)
        {
            strategy.OnShoot(this);
            //예) 리코쳇 : 발사 시 RemainingBounceCount 설정
        }

    }
    public bool LookTarget(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, Layers.GetLayerMask(ELayerName.Enemy));

        if (hitColliders.Length == 0)
        {
            Utils.Log("주변에 적이 없습니다.");
            ReturnPool();
            return false;
        }

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        Collider nearCol = null;

        foreach (Collider hitCollider in hitColliders)
        {
            //이전 타겟과 동일한 경우 패스
            if (mIgnoreColliderIDs.Contains(hitCollider.gameObject.GetInstanceID())) continue;
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            Vector3 targetDir = hitCollider.transform.position - currentPosition;
            float distanceToTarget = targetDir.sqrMagnitude;

            //적이 겹쳐있어 거리가 매우 가까울 때 벡터연산 오류 방지
            if (distanceToTarget < 0.001f) continue;

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestEnemy = hitCollider.transform;
                nearCol = hitCollider;
            }
        }

        if (nearCol == null)
        {
            Utils.Log("맞은 적 외 주변에 적이 없습니다.");
            ReturnPool();
            return false;
        }

        transform.LookAt(closestEnemy.position + mTargetOffset, Vector3.up);
        //Utils.Log($"맞은 적 외 가장 가까운 적 : {closestEnemy.name}");
        return true;
    }

    public void ReturnPool()
    {
        Managers.Pool.ReturnToPool(this);
    }

    //각 전략들에서 데미지 조정 가능
    public void MultipleDamage(float multiplier)
    {
        CurrentDamage *= multiplier;
    }
    public void AddDamage(float amount)
    {
        CurrentDamage += amount;
    }

    //충돌 감지
    private void OnTriggerEnter(Collider other)
    {
        //에디터셋팅 Physics에서 충돌레이어 설정 : Projectile은 Enemy 레이어와만 충돌

        // 1. 최적화 및 버그방지
        int otherID = other.gameObject.GetInstanceID();    //충돌체 오브젝트 아이디
        //충돌했던 놈이면 리턴
        if (mIgnoreColliderIDs.Contains(otherID)) return;


        // 2. IDamageable 체크
        var target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            //현재 리코쳇 때문에 한번 맞은 적들을 계속 기억하지 않음
            //나중에 맞은 적들을 기억해야 할 경우 수정 필요, 기억 안 해도 될 경우는 HashSet말고 int로 처리 할 예정
            mIgnoreColliderIDs.Clear();

            AddIgnoreTarget(otherID);

            bShouldReturnPool = true;   //밑의 전략에서 false로 바꿀 수 있음

            foreach (var strategy in mStrategies)
            {
                //이 프로젝타일이 갖고 있는 전략들 각각에 대해 OnHit 호출
                strategy.OnHit(this, target);
            }

            //Projectile의 현재 데미지만큼만 타겟에게 데미지 입힘
            target.TakeDamage(CurrentDamage);
            Utils.Log($"TargetHit, damage : {CurrentDamage}");

            if (bShouldReturnPool)
            {
                ReturnPool();
            }
        }
        else
        {
            Utils.Log($"충돌은 했으나 IDamageable을 못 찾음! 대상: {other.name}");
        }
    }

    private void Update()
    {
        Move();
        ReturnPoolAfterLifeTime();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * mMoveSpeed * Time.deltaTime);
    }
    private void ReturnPoolAfterLifeTime()
    {
        mLifeTimer += Time.deltaTime;
        if (mLifeTimer >= mLifeTime)
        {
            if (gameObject.activeInHierarchy) ReturnPool();
        }
    }

}
