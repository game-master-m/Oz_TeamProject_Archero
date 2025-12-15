using System.Collections.Generic;
using UnityEngine;

public class LightningFairy : FairyBase
{
    [SerializeField] private LightningFairySkillDataSO mSkillData;

    private HashSet<int> mIgnoreColliderIDs = new HashSet<int>();

    private float mChainRange;
    private float mMaxChainCount;

    //스타트는 테스트 환경에서 작동 확인용 세팅
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetOwner(mPlayer);
        SetUp(mSkillData);
    }

    public void SetUp(LightningFairySkillDataSO skillDataSO)
    {
        //페어리 데이터 스텟 받아오기
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mSeatNumber = skillDataSO.SeatNumber;

        //번개요정 전용 스텟
        mChainRange = skillDataSO.ChainRange;
        mMaxChainCount = skillDataSO.MaxChainCount;

        //자기 자리 위치로 회전
        this.gameObject.transform.RotateAround(mPlayer.gameObject.transform.position, Vector3.up, mSeatAngle * mSeatNumber);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        mIgnoreColliderIDs.Clear();

        Utils.Log("ApplyLightning");
        //번개 데미지 = 데미지 * 0.3(기존 데미지 30%), 맞은 대상 주변에 데미지 주고 튕김(feat.체인 라이트닝)
        float lightningDamage = damage * mDamageDuplicater;

        //맞은 대상 주변 오브젝트 검색
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, mChainRange, Layers.GetLayerMask(ELayerName.Enemy));

        //주변 오브젝트들에 범위 데미지
        foreach (Collider collider in hitColliders)
        {
            IDamageable enemy = collider.gameObject.GetComponent<IDamageable>();

            if (enemy != null)
            {
                Utils.Log($"{hitColliders.Length}개 오브젝트에 범위피해");
                enemy.TakeDamage(lightningDamage);
            }
        }

        //맞은 오브젝트를 중심으로 설정
        Transform centerEnemy = target.gameObject.transform;
        float closestDistance = Mathf.Infinity;
        Vector3 centerPosition = target.gameObject.transform.position;

        Collider nearCol = null;

        for (int i = 0; i < mMaxChainCount; i++)
        {
            int otherID = centerEnemy.gameObject.GetInstanceID();    //충돌체 오브젝트 아이디

            if (mIgnoreColliderIDs.Contains(otherID))
            {
                break; //튕겼던 놈이면 리턴
            }
            else
            {
                //아니면 튕겼던 오브젝트 리스트에 추가
                mIgnoreColliderIDs.Add(otherID);

            }

            //맞은 대상 주변 적 오브젝트 검색
            Collider[] nearColliders = Physics.OverlapSphere(centerEnemy.transform.position, mChainRange, Layers.GetLayerMask(ELayerName.Enemy));

            if (hitColliders.Length == 0)
            {
                //주변 적 없으면 리턴
                return;
            }

            //제일 가까운 적 찾기
            foreach (Collider hitCollider in hitColliders)
            {
                //비활성화된 적은 패스
                if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

                Vector3 targetDir = hitCollider.transform.position - centerPosition;
                float distanceToTarget = targetDir.sqrMagnitude;

                //적이 겹쳐있어 거리가 매우 가까울 때 벡터연산 오류 방지
                if (distanceToTarget < 0.001f) continue;

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    centerEnemy = hitCollider.transform;
                    nearCol = hitCollider;
                }
            }

            if (nearCol == null)
            {
                return;
            }

            //데미지 입힐 수 있으면 데미지 입히기
            IDamageable enemyDamageable = centerEnemy.gameObject.GetComponent<IDamageable>();
            if (enemyDamageable != null) 
            {
                Utils.Log("주변 적으로 튕김");
                enemyDamageable.TakeDamage(lightningDamage);
            }
        }
    }
}
