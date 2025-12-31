using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSphere : Sphere
{
    private PlayerAttack mPlayer;

    [SerializeField] private LaserEffect mLaserEffectPrefab;
    [SerializeField] private float mLaserRadius = 1.0f;

    private LaserEffect mLaserEffect;
    private float mCoolTime = 0;
    private float mLaserDuration;
    private float mLaserRange;
    private float mLaserDelay;

    private bool mIsAttack = false;
    private WaitForSeconds mWaitLaserTick = new WaitForSeconds(0.25f);

    private void Update()
    {
        if (mIsAttack)
        {
            if (mLaserEffect == null)
            {
                mLaserEffect = Managers.Pool.GetFromPool(mLaserEffectPrefab);
                if (mLaserEffect == null) { Utils.Log("레이저 생성 실패"); }
            }
            Vector3 start = this.gameObject.transform.position;
            Vector3 end = start + this.gameObject.transform.forward * mLaserRange;
            mLaserEffect.SetLineRenderer(mLaserRadius);
            mLaserEffect.DrawLaser(start, end);
        }
        else 
        {
            if (Time.time - mCoolTime < mLaserDelay) return;

            LaserAttack();
        }
    }

    public void SetLaser(LaserCircleSkillDataSO skillData, PlayerAttack attack) 
    {
        Managers.Pool.CreatePool(mLaserEffectPrefab, 5, Managers.Pool.transform);
        mLaserDuration = skillData.LaserDuration;
        mLaserRange = skillData.LaserRange;
        mLaserDelay = skillData.LaserDelay;
        mPlayer = attack;   
    }

    public void LaserAttack()
    {
        mIsAttack = true;
        StartCoroutine(LaserAttackCo());
    }

    private IEnumerator LaserAttackCo()
    {
        float startTime = Time.time;
        float laserDamage = mPlayer.Stat.AttackDamage * 0.4f;

        while (Time.time - startTime < mLaserDuration)
        {
            RaycastHit[] hit = Physics.SphereCastAll(this.gameObject.transform.position, mLaserRadius, this.gameObject.transform.forward, mLaserRange);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.TryGetComponent(out EnemyBase enemy))
                {
                    enemy.TakeDamage(laserDamage);
                }
            }
            yield return mWaitLaserTick;
        }
        mLaserEffect.gameObject.SetActive(false);
        Managers.Pool.ReturnToPool(mLaserEffect);
        mLaserEffect = null;
        mIsAttack = false;
        mCoolTime = Time.time;
        Utils.Log($"레이저 공격 : {mIsAttack}");
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * mLaserRange;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(startPos + transform.up * mLaserRadius, endPos + transform.up * mLaserRadius);
        Gizmos.DrawLine(startPos - transform.up * mLaserRadius, endPos - transform.up * mLaserRadius);
        Gizmos.DrawLine(startPos + transform.right * mLaserRadius, endPos + transform.right * mLaserRadius);
        Gizmos.DrawLine(startPos - transform.right * mLaserRadius, endPos - transform.right * mLaserRadius);
        Gizmos.DrawWireSphere(startPos, mLaserRadius);
        Gizmos.DrawWireSphere(endPos, mLaserRadius);
    }
}
