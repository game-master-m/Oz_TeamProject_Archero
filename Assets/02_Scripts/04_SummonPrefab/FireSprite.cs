using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSprite : SpriteBase
{
    //이미 충돌한 충돌체 ID 목록
    private HashSet<int> mIgnoreColliderIDs = new HashSet<int>();

    private List<EnemyBase> mTargetEnemies = new List<EnemyBase>();
    private List<float> mEffectTimes = new List<float>();
    private float mEffectTime = 3;

    private Coroutine fireCoroutine;
    private WaitForSeconds mFireTick = new WaitForSeconds(0.2f);

    //스타트는 테스트 환경에서 작동을 확인용
    private void Start()
    {
        mPlayer = GameObject.FindGameObjectWithTag(Define.Tag_Player).GetComponent<PlayerAttack>();
        SetUp(mPlayer, mSpriteNumber);
    }

    public override void ApplyElement(EnemyBase target, float damage)
    {
        Utils.Log("ApplyElement");
        //화염 데미지 = 데미지 * 0.2(기존 데미지 20%), 3초동안 15틱
        float fireDamage = damage * 0.2f;
        int targetID = target.gameObject.GetInstanceID();

        //맞았던 오브젝트가 아니면 리스트에 추가
        if (!mIgnoreColliderIDs.Contains(targetID))
        {
            mIgnoreColliderIDs.Add(targetID);
            mTargetEnemies.Add(target);
            mEffectTimes.Add(mEffectTime);
        }
        else 
        {
            //맞았던 오브젝트면 효과시간 초기화
            for (int i = 0; i < mTargetEnemies.Count; i++) 
            {
                if (mTargetEnemies[i].gameObject.GetInstanceID() == targetID) 
                {
                    mEffectTimes[i] = mEffectTime;
                }
            }
        }

        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(ApplyFireCo(fireDamage));
        }   
    }

    IEnumerator ApplyFireCo(float damage)
    {
        while (mTargetEnemies.Count > 0) 
        {
            for (int i = mTargetEnemies.Count - 1; i >= 0; i--) 
            {
                if (mEffectTimes[i] <= 0 || !mTargetEnemies[i].gameObject.activeInHierarchy)
                {
                    int id = mTargetEnemies[i].gameObject.GetInstanceID();
                    mEffectTimes.RemoveAt(i);
                    mTargetEnemies.RemoveAt(i);
                    mIgnoreColliderIDs.Remove(id);
                }
                else 
                {
                    mEffectTimes[i] -= 0.2f;
                    //mTargetEnemies[i].TakeDamage(fireDamage);                   
                }
            }
            Utils.Log($"{mTargetEnemies.Count}개 대상에게{damage}데미지");
            yield return mFireTick;
        }

        fireCoroutine = null;
    }
}
