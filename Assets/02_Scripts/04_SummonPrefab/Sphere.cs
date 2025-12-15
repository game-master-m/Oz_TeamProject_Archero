using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private SphereBase mOwner;
    private EnemyBase mTarget;

    // 이번 발사체에서 무시할 충돌체 ID 목록
    private HashSet<int> mIgnoreColliderIDs = new HashSet<int>();

    public void SetOwner(SphereBase owner)
    {
        mOwner = owner;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        int otherID = other.gameObject.GetInstanceID();    //충돌체 오브젝트 아이디
        //충돌했던 놈이면 리턴
        if (mIgnoreColliderIDs.Contains(otherID)) return;

        mIgnoreColliderIDs.Add(otherID);

        if (other.gameObject.GetComponent<EnemyBase>() != null)
        {
            mTarget = other.gameObject.GetComponent<EnemyBase>();
            mOwner.OnHitTarget(mTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int otherID = other.gameObject.GetInstanceID();    //충돌체 오브젝트 아이디
        //충돌했던 놈이면 빼기
        if (mIgnoreColliderIDs.Contains(otherID)) 
        {
            mIgnoreColliderIDs.Remove(otherID);
        }
    }
}
