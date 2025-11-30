using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageble
{
    protected float mCurrentHP;
    protected float mMaxHP;
    public float CurrentHP => mCurrentHP;
    public float MaxHP => mMaxHP;

    public LivingEntity(float maxHp)
    {
        mMaxHP = maxHp;
        mCurrentHP = mMaxHP;
    }

    public void TakeDamage(float amount)
    {
        mCurrentHP -= amount;
        if (mCurrentHP <= 0)
        {
            Die();
        }
    }
    public virtual void Die() { }
}
