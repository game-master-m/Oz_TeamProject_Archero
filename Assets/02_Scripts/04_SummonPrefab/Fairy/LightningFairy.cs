using System.Collections.Generic;
using UnityEngine;

public class LightningFairy : FairyBase
{
    [SerializeField] private LightningFairySkillDataSO mSkillData;
    [SerializeField] private LightningEffect mLightningEffectPrefab;
    [SerializeField] private Vector3 mOffset = new Vector3(2, 1, -3);
    private float mSlerpSpeed = 5.0f;
    private float mChainRange;
    private float mMaxChainCount;

    public void SetUp(LightningFairySkillDataSO skillDataSO, PlayerAttack attack)
    {
        SetOwner(attack);

        Managers.Pool.CreatePool(mLightningEffectPrefab, 10, Managers.Pool.transform);

        //페어리 데이터 스텟 받아오기
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mSeatNumber = skillDataSO.SeatNumber;

        //번개요정 전용 스텟
        mChainRange = skillDataSO.ChainRange;
        mMaxChainCount = skillDataSO.MaxChainCount;

        Utils.Log($"{mPlayer.name}");
        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    private void LateUpdate()
    {
        Vector3 targetPos = mPlayer.gameObject.transform.position + mPlayer.gameObject.transform.rotation * mOffset;
        transform.position = Vector3.Slerp(transform.position, targetPos, mSlerpSpeed * Time.deltaTime);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyLightning");
        //번개 데미지 = 데미지 * 0.3(기존 데미지 30%), 맞은 대상 주변에 데미지 주고 튕김(feat.체인 라이트닝)
        float lightningDamage = damage * mDamageDuplicater;

        //맞은 오브젝트를 중심으로 설정
        Transform currentTarget = target.gameObject.transform;
        HashSet<Transform> hitTargets = new HashSet<Transform>();
        hitTargets.Add(currentTarget);

        for (int i = 0; i < mMaxChainCount; i++)
        {
            float closestDistance = Mathf.Infinity;
            Vector3 centerPosition = currentTarget.gameObject.transform.position;

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
                    damageable.TakeDamage(lightningDamage * FairyReinforceStatic.FairyAttackDamageDuplicater, EDmgElement.Lightning);
                }

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
