using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FairyBase : MonoBehaviour
{
    //현재 작동중인 페어리
    private static List<FairyBase> mAllFaries = new List<FairyBase>();
    //공격 코루틴 호스트
    private static FairyBase mCoroutineHost;
    //공격 코루틴 > 모든 페어리가 공유
    private static Coroutine mAttackCoroutine;

    //페어리용 속성화살
    [SerializeField] protected ElementProjectile mElementsProjectilePrefab;

    //위치 오프셋
    [SerializeField] private Vector3 mTargetOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 mProjectileOffeset = new Vector3(0, 0, 0);

    //적 탐색 사거리 > 이 안에 적 있으면 발사
    [SerializeField] private float mTargetRange = 30.0f;

    //공격모드 > 0 = 레이저, 1 = 화살발사
    [SerializeField] private int mAttackMode = 1; 
    //페어리마다 다른 값을 가짐
    protected float mEffectTime;
    protected float mDamageTick;
    protected float mDamageDuplicater;
    protected int mSeatNumber;

    //페어리 공통
    protected PlayerAttack mPlayer;
    protected static float mAttackSpeed;
    protected static float mAttackDelay = 3.0f;
    protected float mSeatAngle = 45;

    protected Transform mTargetTransform;

    //플레이어 스텟 참조
    private static float mAttackDamage;
    private static float mFairyDamageDuplicater = 1.0f;

    //코루틴용
    private static WaitForSeconds mWaitAttack;

    public void SetOwner(PlayerAttack attack) 
    {
        //플레이어 스텟 받아오기
        mPlayer = attack;
        mAttackDamage = attack.Stat.AttackDamage;
        mAttackSpeed = FairyReinforceStatic.FairyAttackSpeedDuplicater;
        this.gameObject.transform.position = attack.gameObject.transform.position;
        this.gameObject.transform.rotation = Quaternion.identity;

        mWaitAttack = new WaitForSeconds(mAttackDelay / mAttackSpeed);
        mAllFaries.Add(this);

        Managers.Pool.CreatePool(mElementsProjectilePrefab, 50, Managers.Pool.transform);

        if (mAttackCoroutine == null) 
        {
            mCoroutineHost = this;
            mAttackCoroutine = mCoroutineHost.StartCoroutine(GlobalAttackCo());
        }
    }

    private void Update()
    {
        transform.LookAt(mTargetTransform.position, Vector3.up);
    }

    //Projectile과 거의 같음
    public bool LookTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, mTargetRange, Layers.GetLayerMask(ELayerName.Enemy));

        if (hitColliders.Length == 0)
        {
            //Utils.Log("주변에 적이 없습니다.");
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

        mTargetTransform = closestEnemy;
        transform.LookAt(closestEnemy.position + mTargetOffset, Vector3.up);
      
        return true;
    }

    public void SetAttackMode() 
    {
        switch (mAttackMode) 
        {
            case 0:
                ApplyElement(null, mAttackDamage);
                break;

            case 1:
                MakeProjectile();
                break;
        }     
    }

    //발사체 생성
    public void MakeProjectile()
    {
        ElementProjectile elementProjectile = Managers.Pool.GetFromPool(mElementsProjectilePrefab);
        if (elementProjectile != null)
        {
            elementProjectile.gameObject.transform.position = transform.position + mProjectileOffeset;
            elementProjectile.Setup(this, mAttackDamage);
        }
    }

    //풀에 집어넣기 전에 플레이어한테서 떼어내기
    public void Detach() 
    {
        this.gameObject.transform.SetParent(null, false);
        mAllFaries.Remove(this);
        if (mCoroutineHost == this) 
        {
            if (mAllFaries.Count > 0)
            {
                mCoroutineHost = mAllFaries[0];
                mAttackCoroutine = mCoroutineHost.StartCoroutine(GlobalAttackCo());
            }
            else 
            {
                mCoroutineHost = null;
                mAttackCoroutine = null;
            }
        }
    }

    //발사체한테 명중 신호 받았을때
    public void OnHitTarget(EnemyBase target) 
    {
        ApplyElement(target, mAttackDamage * FairyReinforceStatic.FairyAttackDuplicater);
    }

    public abstract void ApplyElement(EnemyBase target, float damage);

    //공격 코루틴
    private static IEnumerator GlobalAttackCo() 
    {
        while (true) 
        {
            foreach (var fairy in mAllFaries) 
            {
                if (fairy != null && fairy.LookTarget()) 
                {
                    fairy.SetAttackMode();
                }
            }
            if (mAttackSpeed != FairyReinforceStatic.FairyAttackSpeedDuplicater) 
            {
                Utils.Log("공속업데이트 완료");
                mAttackSpeed = FairyReinforceStatic.FairyAttackSpeedDuplicater;
                mWaitAttack = new WaitForSeconds(mAttackDelay / mAttackSpeed);
            }
            yield return mWaitAttack;
        }
    }
}
