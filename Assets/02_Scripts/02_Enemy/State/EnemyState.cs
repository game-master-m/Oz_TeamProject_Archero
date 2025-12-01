using UnityEngine;

public class EnemyState : IState
{
    protected readonly EnemyBase mEnemy;
    protected float mElapsedTimeBase = 0f;
    public IState Parent { get; }

    public EnemyState(EnemyBase enemy, IState parent = null)
    {
        this.mEnemy = enemy;
        Parent = parent;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
