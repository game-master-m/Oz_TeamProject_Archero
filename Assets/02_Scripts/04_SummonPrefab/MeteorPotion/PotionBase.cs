using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionBase : MonoBehaviour
{
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
        this.gameObject.transform.position = mSpawnPos;
    }

    public abstract void ApplyPotionEffect();
}
