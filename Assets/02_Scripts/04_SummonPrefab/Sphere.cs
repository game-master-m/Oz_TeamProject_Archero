using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private SphereBase mOwner;
    private EnemyBase mTarget;

    public void SetOwner(SphereBase owner)
    {
        mOwner = owner;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyBase>() != null)
        {
            mTarget = other.gameObject.GetComponent<EnemyBase>();
            mOwner.OnHitTarget(mTarget);
        }
    }
}
