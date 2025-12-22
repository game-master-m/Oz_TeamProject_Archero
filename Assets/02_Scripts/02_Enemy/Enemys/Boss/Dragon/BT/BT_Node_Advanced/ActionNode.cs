using UnityEngine;

public abstract class ActionNode : Node
{
    protected EnemyBase mOwner;
    protected Coroutine mActiveCoroutine;

    public ActionNode(EnemyBase owner) => this.mOwner = owner;

    public override void Abort()
    {
        if (mActiveCoroutine != null)
        {
            mOwner.StopCoroutine(mActiveCoroutine);
            mActiveCoroutine = null;
        }
        base.Abort();
    }
}

