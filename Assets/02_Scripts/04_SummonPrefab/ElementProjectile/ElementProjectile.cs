using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementProjectile : MonoBehaviour
{
    private FairyBase mOwner;
    private EnemyBase mTarget;

    //각각의 프리팹마다 다르게 설정 가능
    [SerializeField] private float mTargetRange = 30.0f;
    [SerializeField] private float mMoveSpeed = 16.0f;
    [SerializeField] private float mLifeTime = 10.0f;
    [SerializeField] private Vector3 mTargetOffset = new Vector3(0, 1.0f, 0);

    //캐싱
    private CapsuleCollider mCollider;

    // 상태 변수
    private float mLifeTimer = 0.0f;

    // 프로퍼티
    public bool bShouldReturnPool { get; set; } = true;
    public float CurrentDamage { get; private set; }
    public CapsuleCollider Collider => mCollider;

    private void Awake()
    {
        mCollider = GetComponent<CapsuleCollider>();
    }
    private void OnEnable()
    {
        mLifeTimer = 0.0f;
    }

    //PlayerAttack이 갖고 있는 strategies를 주입
    public void Setup(FairyBase owner, float damage)
    {
        mOwner = owner;
        CurrentDamage = damage;

        //각각 발사되는 프로젝타일 변수 초기화
        bShouldReturnPool = true;

        //적이 없으면 발사하지 않음
        if (!LookTarget(mTargetRange))
        {
            return;
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
        // 1. IDamageable 체크
        var target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            bShouldReturnPool = true;   //밑의 전략에서 false로 바꿀 수 있음

            //Projectile의 현재 데미지만큼만 타겟에게 데미지 입힘
            target.TakeDamage(CurrentDamage, EDmgElement.Normal);
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

        // 2. EnemyBase 체크
        if (other.gameObject.GetComponent<EnemyBase>() != null)
        {
            mTarget = other.gameObject.GetComponent<EnemyBase>();
            mOwner.OnHitTarget(mTarget);
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
