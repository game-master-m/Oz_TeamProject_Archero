using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCircle : SphereBase
{
    [SerializeField] private LightningCircleSkillDataSO mSkillDataSO;
    [SerializeField] private LightningEffect mLightningEffectPrefab;

    [SerializeField] private int mOrbitIndex = 0;
    private int mOrbitCount = 0;
    private float mChainRange;
    private float mMaxChainCount;
    private float mDamageDuplicater;

    void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillDataSO);
    }

    void Update()
    {
        if (mOrbitCount != mTotalCount)
        {
            float angle = (180 / (mTotalCount / 2)) * mOrbitIndex;
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
            mOrbitCount = mTotalCount;
        }
        this.gameObject.transform.position = mPlayer.gameObject.transform.position + mPositionOffset;
        this.transform.Rotate(Vector3.up * mRotateSpeed * Time.deltaTime);
    }

    public void SetUp(LightningCircleSkillDataSO skillDataSO)
    {
        mOrbitCount = mTotalCount;
        mOrbitIndex = mTotalCount/2;
        Managers.Pool.CreatePool(mLightningEffectPrefab, 10, Managers.Pool.transform);
        mPositionOffset = skillDataSO.PositionOffset;
        mRotateSpeed = skillDataSO.RotateSpeed;
        mDamageDuplicater = skillDataSO.DamageDuplicater;

        mChainRange = skillDataSO.ChainRange;
        mMaxChainCount = skillDataSO.MaxChainCount;
    }

    public override void ApplyDamage(EnemyBase target, float damage)
    {
        float sphereDamage = damage * 1.25f;
        float lightningDamage = damage * mDamageDuplicater;

        Utils.Log($"구체데미지{sphereDamage}");
        target.TakeDamage(sphereDamage);

        //맞은 오브젝트를 중심으로 설정
        Transform currentTarget = target.gameObject.transform;
        HashSet<Transform> hitTargets = new HashSet<Transform>();
        hitTargets.Add(currentTarget);

        float closestDistance = Mathf.Infinity;
        Vector3 centerPosition = target.gameObject.transform.position;

        for (int i = 0; i < mMaxChainCount; i++)
        {
            //맞은 대상 주변 적 오브젝트 검색
            Collider[] hitColliders = Physics.OverlapSphere(currentTarget.transform.position, mChainRange, Layers.GetLayerMask(ELayerName.Enemy));

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
                IDamageable damageable = nextTarget.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(lightningDamage);
                }

                Utils.Log("이펙트소환!!");
                //라인렌더러 이펙트
                LightningEffect lightning = Managers.Pool.GetFromPool(mLightningEffectPrefab);
                lightning.DrawLightning(currentTarget.position, nextTarget.position);

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
