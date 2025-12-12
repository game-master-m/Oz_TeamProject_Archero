using UnityEngine;
using System.Collections;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    protected float mCurrentHP;
    protected float mMaxHP;
    public float CurrentHP => mCurrentHP;
    public float MaxHP => mMaxHP;

    //인터페이스 IDamageable 구현
    public bool IsDead => mCurrentHP <= 0.0f;

    private bool bIsDead = false;

    private Coroutine mDotDamageCo;

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

    //인터페이스(IDamageable 구현)
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

    //도트 데미지 받기 추가
    public virtual void TakeDotDamage(float damage, float duration, float damageTick) 
    {
        if (bIsDead) return;

        //이미 도트데미지 받는중이면 중단하고 다시시작 > 지속시간 갱신
        if (mDotDamageCo != null)
        {
            StopCoroutine(mDotDamageCo);
            mDotDamageCo = null;
        }
        mDotDamageCo = StartCoroutine(DotDamageCo(damage, duration, damageTick));
    }

    IEnumerator DotDamageCo(float damage, float duration, float damageTick)
    {
        WaitForSeconds waitDamageTick = new WaitForSeconds(damageTick);
        float timer = 0f;

        while (timer < duration) 
        {
            TakeDamage(damage);
            if (bIsDead) break;
            yield return waitDamageTick;
            timer += Time.deltaTime;
        }

        mDotDamageCo = null;
    }

    public virtual void Die()
    {
        // 자식 클래스에서 구현 (애니메이션, 풀 반환 등)

    }
}
