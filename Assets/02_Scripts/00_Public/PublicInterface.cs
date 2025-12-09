using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
    bool IsDead { get; }
}

public interface IProjectileStrategy
{
    void OnShoot(Projectile projectile);
    void OnHit(Projectile projectile, IDamageable target);
}

public interface IPassiveStrategy
{
    void OnEquip(PlayerAttack attack);
    void OnUpdate(PlayerAttack attack);
    void OnUnequip(PlayerAttack attack);
}

//스택 가능 한 스킬은 이 인터페이스를 구현
public interface ISkillStackable<T>
{
    bool TryStack(T strategy);
}

