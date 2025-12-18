using UnityEngine;
using System.Collections;
using System;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    protected float mCurrentHP;
    protected float mMaxHP;
    public float CurrentHP => mCurrentHP;
    public float MaxHP => mMaxHP;

    //인터페이스 IDamageable 구현
    public bool IsDead => mCurrentHP <= 0.0f;

    //UI관련
    public event Action<float, EDmgElement, bool> onDmgTaken;   //데미지, 속성종류, 크리티컬 여부
    public event Action<float> onHPChanged; //float 은 0~1값

    private bool bIsDead = false;

    private Coroutine mDotDamageCo;

    protected virtual void OnEnable()
    {
        // 오브젝트가 켜질 때마다 체력 리셋 (풀링 사용할 때 필수)
        mCurrentHP = mMaxHP;
        bIsDead = false;

        onHPChanged?.Invoke(1.0f);  //풀에서 꺼내질 때, 처음 100%
    }

    // 외부에서 스탯을 덮어씌워야 할 때 호출 (예: 레벨업 후 스폰)
    public void Init(float maxHp)
    {
        mMaxHP = maxHp;
        mCurrentHP = mMaxHP;

        onHPChanged?.Invoke(1.0f);
    }

    //인터페이스(IDamageable 구현)
    public virtual void TakeDamage(float amount)
    {
        TakeDamage(amount, EDmgElement.Normal, false);
    }
    public virtual void TakeDamage(float amount, EDmgElement element, bool isCritical = false)
    {
        if (bIsDead) return;

        mCurrentHP -= amount;

        //맞았을 때 이벤트 발행
        onDmgTaken?.Invoke(amount, element, isCritical);
        onHPChanged?.Invoke(mCurrentHP / mMaxHP);

        if (mCurrentHP <= 0)
        {
            mCurrentHP = 0;
            onHPChanged?.Invoke(0.0f);
            bIsDead = true;
            Die();
        }
    }
    public virtual void TakeDotDamage(float damage, float duration, float damageTick)
    {
        TakeDotDamage(damage, duration, damageTick, EDmgElement.Normal);
    }
    //도트 데미지 받기 추가
    public virtual void TakeDotDamage(float damage, float duration, float damageTick, EDmgElement element)
    {
        if (bIsDead) return;

        //이미 도트데미지 받는중이면 중단하고 다시시작 > 지속시간 갱신
        if (mDotDamageCo != null)
        {
            StopCoroutine(mDotDamageCo);
            mDotDamageCo = null;
        }
        mDotDamageCo = StartCoroutine(DotDamageCo(damage, duration, damageTick, element));
    }

    IEnumerator DotDamageCo(float damage, float duration, float damageTick, EDmgElement element)
    {
        WaitForSeconds waitDamageTick = new WaitForSeconds(damageTick);
        float timer = 0f;

        while (timer < duration)
        {
            TakeDamage(damage, element);
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
