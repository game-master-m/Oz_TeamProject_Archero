using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMeteor : MeteorBase
{
    [SerializeField] private ExplodeEffect mExplodeEffectPrefab;
    [SerializeField] private WarningCircleEffect mCircleEffectPrefab;
    private ExplodeEffect mExplodeEffect;
    private WarningCircleEffect mCircleEffect;

    private float mToxicDamage;
    private float mEffectTime;
    private float mDamageTick;

    //세팅
    public void SetUp(ToxicMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mRange = skillDataSO.DamageRange;
        mMeteorDamage = attack.Stat.AttackDamage + PublicDamageConstans.MeteorDamageDuplicater;
        mToxicDamage = attack.Stat.AttackDamage * skillDataSO.DamageDuplicater;
        mEffectTime = skillDataSO.EffectTime;
        mDamageTick = skillDataSO.DamageTick;
        mMeteorSpeed = skillDataSO.MeteorSpeed;
        Utils.Log("메테오 셋업 완료");

        Managers.Pool.CreatePool(mExplodeEffectPrefab, 3, Managers.Pool.transform);
        Managers.Pool.CreatePool(mCircleEffectPrefab, 3, Managers.Pool.transform);

        SetWarningEffect();
    }

    //속성 부여
    public override void Applyelement(EnemyBase enemy)
    {
        enemy.TakeDotDamage(mToxicDamage, mEffectTime, mDamageTick, EDmgElement.Poison);
    }

    public override void ReturnPool()
    {
        Managers.Pool.ReturnToPool(mExplodeEffect);
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
        Managers.Pool.ReturnToPool(mCircleEffect);

        mExplodeEffect = Managers.Pool.GetFromPool(mExplodeEffectPrefab);
        mExplodeEffect.transform.localScale = Vector3.one * mRange;
        mExplodeEffect.gameObject.transform.position
            = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);

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
