using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CloudFootedStrategy : IPassiveStrategy
{
    private KnockBack_Effect mEffectPrefab;
    private KnockBack_Effect mEffect;
    private List<ParticleSystem> mParticles = new List<ParticleSystem>();
    private float mEffectDuration;
    private bool mIsEffectPlaying = false;

    private List<NavMeshAgent> mAgents = new List<NavMeshAgent>();

    private CloudFootedSkillDataSO mSkillData;
    private PlayerAttack mPlayer;

    private int mDamageCount;
    private float mDamageTick;
    private float mDamageDuplicater;
    private float mDamageDelay;

    private float mKnockBackRadius;
    private float mKnockBackForce;

    private float mStartTime = 0;
    private float mEffectTime = 0;

    public CloudFootedStrategy(CloudFootedSkillDataSO skillDataSO)
    {
        mSkillData = skillDataSO;
        mEffectPrefab = skillDataSO.EffectPrefab;
        mEffectDuration = skillDataSO.EffectDuration;

        mDamageCount = skillDataSO.DamageCount;
        mDamageTick = skillDataSO.DamageTick;
        mDamageDuplicater = skillDataSO.DamageDuplicater;
        mDamageDelay = skillDataSO.DamageDelay;

        mKnockBackRadius = skillDataSO.KnockBackRadius;
        mKnockBackForce = skillDataSO.KnockBackForce;
    }

    public void OnEquip(PlayerAttack attack)
    {
        Managers.Pool.CreatePool(mEffectPrefab, 1, Managers.Pool.transform);
        mPlayer = attack;

        mEffect = Managers.Pool.GetFromPool(mEffectPrefab);
        mParticles.AddRange(mEffect.GetComponentsInChildren<ParticleSystem>());
        mEffect.gameObject.transform.SetParent(mPlayer.gameObject.transform);
        mEffect.transform.localPosition = Vector3.zero;
        mEffect.gameObject.SetActive(false);
    }

    public void OnUpdate(PlayerAttack attack)
    {
        if (Time.time - mStartTime > mDamageDelay)
        {
            mStartTime = Time.time;
            ActiveKnockBack(attack);
        }

        if (mIsEffectPlaying)
        {
            if (Time.time - mEffectTime > mEffectDuration)
            {
                mEffect.gameObject.SetActive(false);
                mIsEffectPlaying = false;
                ResumeAgents();
            }
        }
    }

    public void OnUnequip(PlayerAttack attack)
    {
        mStartTime = 0;
    }

    private void ActiveKnockBack(PlayerAttack attack)
    {
        if (mEffectPrefab != null)
        {
            mEffect.gameObject.SetActive(true);

            foreach (var ps in mParticles) { ps.Play(); }

            mIsEffectPlaying = true;
            mEffectTime = Time.time;
        }

        float knockbackDamage = attack.Stat.AttackDamage * mDamageDuplicater;
        Collider[] hitColliders = Physics.OverlapSphere(mPlayer.gameObject.transform.position, mKnockBackRadius, Layers.GetLayerMask(ELayerName.Enemy));

        foreach (Collider collider in hitColliders)
        {
            if (!collider.enabled || !collider.gameObject.activeInHierarchy) continue;

            if (collider.gameObject.TryGetComponent(out NavMeshAgent agent))
            {
                Vector3 dir = (collider.gameObject.transform.position - mPlayer.gameObject.transform.position).normalized;
                
                agent.isStopped = true;

                Vector3 knockBackPos = collider.transform.position + dir * mKnockBackForce;

                if (NavMesh.SamplePosition(knockBackPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) 
                {
                    collider.gameObject.transform.position = hit.position;
                }

                mAgents.Add(agent);
            }

            if (collider.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDotDamage(knockbackDamage, mDamageTick * mDamageCount, mDamageTick);
            }
        }
    }

    private void ResumeAgents() 
    {
        for (int i = 0; i < mAgents.Count; i++) 
        {
            mAgents[i].isStopped = false;
        }

        mAgents.Clear();
    }

    void OnDrawGizmos() 
    {
        if (mPlayer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(mPlayer.transform.position, mKnockBackRadius);
        }
    }
}
