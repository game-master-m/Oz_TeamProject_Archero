using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);
    bool IsDead { get; }
}

public interface IProjectileStrategy
{
    void OnShoot(GameObject projectile);
    void OnHit(GameObject projectile, IDamageable target);
}

public interface IPassiveStrategy
{
    void OnEquip(GameObject player);
    void OnUpdate(GameObject player);
    void OnUnequip(GameObject player);
}

