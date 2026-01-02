using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionBase : MonoBehaviour
{
    private Vector3 mOffset = new Vector3(0f, 0.5f, 0f);
    private Vector3 mSpawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Define.Tag_Player)) 
        {
            ApplyPotionEffect();
        }
    }

    //화면 내 땅바닥 무작위 위치에 생성됨
    public void SetPosition(PlayerAttack attack)
    {
        //레이 쏴서 당바닥 감지
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(Random.Range(0.1f,0.9f), Random.Range(0.1f,0.9f), 0));

        float planeY = 0f;
        //위치 구하는 공식
        float temp = (planeY - ray.origin.y) / ray.direction.y;
        mSpawnPos = ray.origin + ray.direction * temp;  
        this.gameObject.transform.position = mSpawnPos + mOffset;
    }

    public abstract void ApplyPotionEffect();

    protected Transform FindCloseEnemy() 
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 80, Layers.GetLayerMask(ELayerName.Enemy));

        if (hitColliders.Length == 0)
        {
            Utils.Log("주변에 적이 없습니다.");
            return null;
        }

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        Collider nearCol = null;

        foreach (Collider hitCollider in hitColliders)
        {
            //비활성화된 적은 패스
            if (!hitCollider.enabled || !hitCollider.gameObject.activeInHierarchy) continue;

            Vector3 targetDir = hitCollider.transform.position - currentPosition;
            float distanceToTarget = targetDir.sqrMagnitude;

            //적이 겹쳐있어 거리가 매우 가까울 때 벡터연산 오류 방지
            if (distanceToTarget < 0.001f) continue;

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestEnemy = hitCollider.transform;
                nearCol = hitCollider;
            }
        }

        return closestEnemy;
    }
}
