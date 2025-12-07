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

