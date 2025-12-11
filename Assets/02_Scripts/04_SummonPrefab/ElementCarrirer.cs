using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementCarrirer : MonoBehaviour
{
    private GameObject mOwner;
    private EnemyBase mTarget;

    //private int mApplyElement = 0;
    //private float mApplyDamage = 0;

    public void SetOwner(GameObject owner) 
    {
        mOwner = owner; 
    }

    //public void SetUp(int element, float damage) 
    //{
    //    mApplyElement = element;
    //    mApplyDamage = damage;
    //}

    private void OnTriggerEnter(Collider other)
    {
        //var target = other.GetComponent<ElementApplicator>();
        //if (target != null)
        //{
        //    target.ApplyElements(mApplyElement, mApplyDamage);
        //}

        if (other.gameObject.GetComponent<EnemyBase>() != null) 
        {
            mTarget = other.gameObject.GetComponent<EnemyBase>();
            
            mOwner.GetComponent<SpriteBase>().OnHitTarget(mTarget);
        }
    }
}
