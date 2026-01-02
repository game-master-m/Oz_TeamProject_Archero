using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFairy : FairyBase
{
    [SerializeField] private SuperFairySkillDataSO mSkillData;
    [SerializeField] private Vector3 mOffset = new Vector3(-2, 1, -3);

    [SerializeField] private LaserEffect mLaserEffectPrefab;
    [SerializeField] private float mLaserRadius = 2.0f;
    private LaserEffect mLaserEffect;
    private List<Transform> mPointList = new List<Transform>();

    private float mSlerpSpeed = 5.0f;

    private float mLaserDuration;
    private float mLaserDamageTick;
    private float mLaserRange;
    private float mLaserDamageDuplicater;
    private float mLaserChainCount;

    private bool mIsAttack = false;

    private WaitForSeconds mLaserDelay;

    public void SetUp(SuperFairySkillDataSO skillDataSO, PlayerAttack attack)
    {
        SetOwner(attack);

        Managers.Pool.CreatePool(mLaserEffectPrefab, 10, Managers.Pool.transform);

        //페어리 데이터 스텟 받아오기
        mSeatNumber = skillDataSO.SeatNumber;
        mLaserDuration = skillDataSO.LaserDuration;
        mLaserDamageTick = skillDataSO.LaserDamageTick;
        mLaserRange = skillDataSO.LaserRange;
        mLaserDamageDuplicater = skillDataSO.LaserDamageDuplicater;
        mLaserChainCount = skillDataSO.LaserChainCount;

        mLaserDelay = new WaitForSeconds(mLaserDamageTick);

        Utils.Log($"{mPlayer.name}");
        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    private void Update()
    {
        if (mIsAttack)
        {
            if (mLaserEffect == null)
            {
                mLaserEffect = Managers.Pool.GetFromPool(mLaserEffectPrefab);
                if (mLaserEffect == null) { Utils.Log("레이저 생성 실패"); }
            }
            mLaserEffect.SetLineRenderer(mLaserRadius);
            mLaserEffect.DrawLaser(mPointList);
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mPlayer.gameObject.transform.position + mPlayer.gameObject.transform.rotation * mOffset;
        transform.position = Vector3.Slerp(transform.position, targetPos, mSlerpSpeed * Time.deltaTime);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        if (LookTarget())
        {
            Utils.Log("슈퍼레이저발사");
            LaserAttack(damage);
        }
    }

    public void LaserAttack(float damage)
    {
        mIsAttack = true;
        StartCoroutine(LaserAttackCo(damage));
    }

    private IEnumerator LaserAttackCo(float damage)
    {
        float startTime = Time.time;
        float laserDamage = damage * mLaserDamageDuplicater;

        while (Time.time - startTime < mLaserDuration)
        {
            TargetPointSearch();
            for (int i = 0; i < mPointList.Count; i++)
            {
                if (mPointList[i].TryGetComponent(out EnemyBase enemy))
                {
                    enemy.TakeDamage(laserDamage * FairyReinforceStatic.FairyAttackDamageDuplicater);
                }
            }
            yield return mLaserDelay;
        }
        if (mLaserEffect == null) Utils.Log("레이저없음");
        Managers.Pool.ReturnToPool(mLaserEffect);
        mLaserEffect = null;
        mIsAttack = false;
        Utils.Log($"레이저 공격 : {mIsAttack}");
    }

    public void TargetPointSearch()
    {
        Transform currentTarget = this.gameObject.transform;
        HashSet<Transform> hitTargets = new HashSet<Transform>();
        hitTargets.Add(currentTarget);

        mPointList.Clear();
        mPointList.Add(this.gameObject.transform);

        for (int i = 0; i < mLaserChainCount; i++)
        {
            float closestDistance = Mathf.Infinity;
            Vector3 centerPosition = currentTarget.gameObject.transform.position;

            //맞은 대상 주변 적 오브젝트 검색
            Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, mLaserRange, Layers.GetLayerMask(ELayerName.Enemy));

            Transform nextTarget = null;

            if (hitColliders.Length == 0)
            {
                //주변 적 없으면 탈출
                break;
            }

            //제일 가까운 적 찾기
            foreach (Collider hitCollider in hitColliders)
            {
                //비활성화된 적은 패스
                if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;
                if (hitCollider.transform == currentTarget) continue;
                if (hitTargets.Contains(hitCollider.transform)) continue;

                Vector3 targetDir = hitCollider.transform.position - centerPosition;
                float distanceToTarget = targetDir.sqrMagnitude;

                //적이 겹쳐있어 거리가 매우 가까울 때 벡터연산 오류 방지
                if (distanceToTarget < 0.001f) continue;

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    nextTarget = hitCollider.transform;
                }
            }

            if (nextTarget != null)
            {
                mPointList.Add(nextTarget.transform);
                hitTargets.Add(nextTarget);
                currentTarget = nextTarget;
            }
            else
            {
                break;
            }
        }
    }
}
