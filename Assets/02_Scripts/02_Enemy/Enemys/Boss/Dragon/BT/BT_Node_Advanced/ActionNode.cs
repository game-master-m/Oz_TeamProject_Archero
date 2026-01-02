using UnityEngine;
using System.Collections;
public abstract class ActionNode : Node
{
    protected EnemyBase mOwner;
    protected Coroutine mActiveCoroutine;

    public ActionNode(EnemyBase owner) => this.mOwner = owner;

    protected void StartActionCoroutine(IEnumerator routine)
    {
        StopActionCoroutine(); // 이미 실행 중인 게 있다면 안전하게 중단
        mActiveCoroutine = mOwner.StartCoroutine(routine);
    }

    protected void StopActionCoroutine()
    {
        if (mActiveCoroutine != null)
        {
            mOwner.StopCoroutine(mActiveCoroutine);
            mActiveCoroutine = null;
        }
    }
    public override void Abort()
    {
        StopActionCoroutine();
        base.Abort();
    }
}

