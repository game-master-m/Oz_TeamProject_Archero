using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BoltMeteor : MeteorBase
{
    [SerializeField] private ExplodeEffect mExplodeEffectPrefab;
    [SerializeField] private WarningCircleEffect mCircleEffectPrefab;
    [SerializeField] private LightningEffect mLightningEffectPrefab;
    private ExplodeEffect mExplodeEffect;
    private WarningCircleEffect mCircleEffect;

    private int mMaxChainCount;
    private float mChainRange;
    private float mBoltDamage;

    //세팅
    public void SetUp(BoltMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mRange = skillDataSO.DamageRange;
        mChainRange = skillDataSO.ChainRange;
        mMaxChainCount = skillDataSO.MaxChainCount;
        mMeteorDamage = attack.Stat.AttackDamage + PublicDamageConstans.MeteorDamageDuplicater;
        mBoltDamage = attack.Stat.AttackDamage * skillDataSO.DamageDuplicater;
        mMeteorSpeed = skillDataSO.MeteorSpeed;
        Managers.Pool.CreatePool(mLightningEffectPrefab, 8, Managers.Pool.transform);
        Utils.Log("메테오 셋업 완료");

        Managers.Pool.CreatePool(mExplodeEffectPrefab, 8, Managers.Pool.transform);
        Managers.Pool.CreatePool(mCircleEffectPrefab, 8, Managers.Pool.transform);

        SetWarningEffect();
    }

    //속성 부여
    public override void Applyelement(EnemyBase enemy)
    {
        enemy.TakeDamage(mMeteorDamage);

        //맞은 오브젝트를 중심으로 설정
        Transform currentTarget = enemy.gameObject.transform;
        HashSet<Transform> hitTargets = new HashSet<Transform>();
        hitTargets.Add(currentTarget);

        for (int i = 0; i < mMaxChainCount; i++)
        {
            float closestDistance = Mathf.Infinity;
            Vector3 centerPosition = enemy.gameObject.transform.position;
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
                    damageable.TakeDamage(mBoltDamage, EDmgElement.Lightning);
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

    public override void ReturnPool()
    {
        if (mCircleEffect != null)
        {
            Managers.Pool.ReturnToPool(mCircleEffect);
            mCircleEffect = null;
        }
        if (mExplodeEffect != null)
        {
            Managers.Pool.ReturnToPool(mExplodeEffect);
            mExplodeEffect = null;
        }
        Managers.Pool.ReturnToPool(this);
    }

    private void SetWarningEffect()
    {
        mCircleEffect = Managers.Pool.GetFromPool(mCircleEffectPrefab);
        mCircleEffect.transform.localScale = Vector3.one * mRange * 2f;
        mCircleEffect.gameObject.transform.position
            = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);
    }

    protected override void SetExplodeEffect()
    {
        mExplodeEffect = Managers.Pool.GetFromPool(mExplodeEffectPrefab);
        mExplodeEffect.transform.localScale = Vector3.one * mRange;
        mExplodeEffect.gameObject.transform.position
            = new Vector3(this.gameObject.transform.position.x, 0.2f, this.gameObject.transform.position.z);

        if (mExplodeEffect != null)
        {
            mExplodeEffect.gameObject.SetActive(true);
            ParticleSystem[] particles = mExplodeEffect.gameObject.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particles) { ps.Play(); }
        }

        StartCoroutine(EffectCo());
    }

    IEnumerator EffectCo()
    {
        yield return mWaitEffect;

        ReturnPool();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mRange);

        if (mCircleEffect != null)
        {
            Gizmos.color = Color.green;
            float radius = mCircleEffect.transform.localScale.x * 0.5f;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        if (mExplodeEffect != null)
        {
            Gizmos.color = Color.blue;
            float radius = mExplodeEffect.transform.localScale.x * 0.5f;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
