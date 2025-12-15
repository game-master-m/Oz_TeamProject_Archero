using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FairyBase : MonoBehaviour
{
    //페어리용 속성화살
    [SerializeField] protected ElementProjectile mElementsProjectilePrefab;

    //위치 오프셋
    [SerializeField] private Vector3 mTargetOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 mProjectileOffeset = new Vector3(0, -0.5f, 0);
    [SerializeField] private Vector3 mPositionOffset = new Vector3(1.5f, 1.0f, 0);

    //적 탐색 사거리 > 이 안에 적 있으면 발사
    [SerializeField] private float mTargetRange = 30.0f;

    //페어리마다 다른 값을 가짐
    protected float mEffectTime;
    protected float mDamageTick;
    protected float mDamageDuplicater;
    protected int mSeatNumber;
    protected int mElementNumber;

    //페어리 공통
    protected PlayerAttack mPlayer;
    protected float mSeatAngle = 60;

    //플레이어 스텟 참조
    private float mAttackDamage;
    private float mAttackSpeed;

    //코루틴용
    private WaitForSeconds mWaitAttack;

    public void SetOwner(PlayerAttack attack) 
    {
        //플레이어 스텟 받아오기
        mPlayer = attack;
        mAttackSpeed = attack.gameObject.GetComponent<PlayerStat>().AttackSpeed;
        mAttackDamage = attack.gameObject.GetComponent<PlayerStat>().AttackDamage * 0.4f;

        //위치 설정
        this.gameObject.transform.SetParent(attack.gameObject.transform, false);      
        this.gameObject.transform.position = attack.gameObject.transform.position + mPositionOffset;

        mWaitAttack = new WaitForSeconds(mAttackSpeed);

        Managers.Pool.CreatePool(mElementsProjectilePrefab, 50, Managers.Pool.transform);

        //공격 시작
        StartCoroutine(AttackCo());
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

        transform.LookAt(closestEnemy.position + mTargetOffset, Vector3.up);
      
        return true;
    }

    //발사체 생성
    public void MakeProjectile()
    {
        ElementProjectile elementProjectile = Managers.Pool.GetFromPool(mElementsProjectilePrefab);
        if (elementProjectile != null)
        {
            elementProjectile.transform.position = transform.position + mProjectileOffeset;
            elementProjectile.Setup(this, mAttackDamage);
        }
    }

    //풀에 집어넣기 전에 플레이어한테서 떼어내기
    public void Detach() 
    {
        this.gameObject.transform.SetParent(null, false);
    }

    //발사체한테 명중 신호 받았을때
    public void OnHitTarget(EnemyBase target) 
    {
        ApplyElement(target, mAttackDamage);
    }

    public abstract void ApplyElement(EnemyBase target, float damage);

    //데미지 UP
    public void DuplicateDamage(float amount) 
    {
        mAttackDamage = mAttackDamage + (mAttackDamage * amount);
    }

    //공격속도 UP
    public void DuplicateSpeed(float amount) 
    {
        mAttackSpeed = mAttackSpeed + (mAttackSpeed * amount);
    }

    //공격 코루틴
    IEnumerator AttackCo() 
    {
        while (true) 
        {            
            if (LookTarget())
            {
                MakeProjectile();
            }

            yield return mWaitAttack;
        }
    }
}
