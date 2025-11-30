using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    protected float mCurrentHP;
    protected float mMaxHP;
    public float CurrentHP => mCurrentHP;
    public float MaxHP => mMaxHP;
    public bool IsDead => mCurrentHP <= 0.0f;

    private bool bIsDead = false;

    protected virtual void OnEnable()
    {
        // 오브젝트가 켜질 때마다 체력 리셋 (풀링 사용할 때 필수)
        mCurrentHP = mMaxHP;
        bIsDead = false;


    }

    // 외부에서 스탯을 덮어씌워야 할 때 호출 (예: 레벨업 후 스폰)
    public void Init(float maxHp)
    {
        mMaxHP = maxHp;
        mCurrentHP = mMaxHP;
    }

    public virtual void TakeDamage(float amount)
    {
        if (bIsDead) return;

        mCurrentHP -= amount;

        if (mCurrentHP <= 0)
        {
            mCurrentHP = 0;
            bIsDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        // 자식 클래스에서 구현 (애니메이션, 풀 반환 등)

    }
}
