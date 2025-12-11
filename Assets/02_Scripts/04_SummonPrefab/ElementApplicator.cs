using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementApplicator : MonoBehaviour
{
    private float mTimer = 0.0f;
    private float mTargetRange = 10.0f;
    private WaitForSeconds mFireTick = new WaitForSeconds(0.2f);
    private WaitForSeconds mPoisonTick = new WaitForSeconds(1.0f);
    private EnemyBase mEnemy;

    //0 = 화염, 1 = 얼음, 2 = 번개, 3 = 독
    public void ApplyElements(int elements, float damage) 
    {
        switch (elements) 
        {
            case 0:
                StopCoroutine(ApplyFireCo(damage));
                StartCoroutine(ApplyFireCo(damage));
                break;
            case 1:
                StopCoroutine(ApplyIceCo(damage));
                StartCoroutine(ApplyIceCo(damage));
                break;
            case 2:
                ApplyThunder(elements, damage);
                break;
            case 3:
                StopCoroutine(ApplyPoisonCo(damage));
                StartCoroutine(ApplyPoisonCo(damage));
                break;
        }

        mEnemy = this.gameObject.GetComponent<EnemyBase>();
    }

    IEnumerator ApplyFireCo(float damage) 
    {
        //화염 데미지 = 데미지 * 0.2(기존 데미지 20%), 3초동안 15틱
        float fireDamage = damage * 0.2f;

        for (int i = 0; i <= 15; i++) 
        {
            //mEnemy.TakeDamage(fireDamage);
            Utils.Log($"{fireDamage}");

            yield return mFireTick;
        }
    }

    IEnumerator ApplyIceCo(float damage)
    {
        return null;
    }

    private void ApplyThunder(int elements, float damage) 
    {
        //번개데미지 = 데미지 * 0.3(기존 데미지의 30%), 대상과 대상 주변
        float thunderDamage = damage * 0.3f;

        //대상에게 데미지
        mEnemy.TakeDamage(thunderDamage);

        //주변 대상 가져오기
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, mTargetRange, Layers.GetLayerMask(ELayerName.Enemy));

        if (hitColliders.Length == 0)
        {
            Utils.Log("주변에 적이 없습니다.");
            return;
        }

        foreach (Collider hitCollider in hitColliders)
        {
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            hitCollider.gameObject.GetComponent<EnemyBase>().TakeDamage(thunderDamage);
        }      
    }

    IEnumerator ApplyPoisonCo(float damage)
    {
        float poisonDamage = damage * 0.5f;

        while (true) 
        {
            mEnemy.TakeDamage(poisonDamage);

            yield return mPoisonTick;
        }
    }
}
