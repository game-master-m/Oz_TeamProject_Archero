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

//스택(중복선택) 가능 한 스킬은 이 인터페이스를 구현
public interface ISkillStackable<T> : IStackable
{
    bool TryStack(T strategy);
}
//스킬선택 창에서 중복 가능한 스킬인지 보고 선택 됐으면 앞으로 보여줄 스킬목록에서 삭제하기위한 인터페이스
public interface IStackable { }

