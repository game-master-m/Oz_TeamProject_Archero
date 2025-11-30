using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyBase : LivingEntity
{
    public EnemyBase(float maxHp) : base(maxHp)
    {
    }

    private void Awake()
    {

    }

    public override void Die()
    {

    }
}
