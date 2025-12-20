using UnityEngine;

public abstract class ActionNode : Node
{
    protected MonoBehaviour owner;
    protected Coroutine activeCoroutine;

    public ActionNode(MonoBehaviour owner) => this.owner = owner;

    public override void Abort()
    {
        if (activeCoroutine != null)
        {
            owner.StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
        base.Abort();
    }
}

