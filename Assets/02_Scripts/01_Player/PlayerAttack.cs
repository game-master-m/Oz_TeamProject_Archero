using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Projectile mProjectilePrefab;
    [SerializeField] private Vector3 mProjectileOffeset = new Vector3(0, 1.0f, 0);

    private List<IProjectileStrategy> mArrowStrategies = new List<IProjectileStrategy>();
    private List<IPassiveStrategy> mPassiveStrategies = new List<IPassiveStrategy>();


    public float AttackDamage { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackSpeed { get; set; }  //초당 공격 횟수
    public bool IsAutoTurret { get; set; } = false;

    private void Start()
    {
        Managers.Pool.CreatePool(mProjectilePrefab, 100, Managers.Pool.transform);

        mArrowStrategies.Add(new BasicArrowStrategy());
    }
    private void Update()
    {
        foreach (var passive in mPassiveStrategies)
        {
            passive.OnUpdate(this);
        }
    }
    public void InitStat(PlayerStat stats)
    {
        AttackDamage = stats.AttackDamage;
        AttackSpeed = stats.AttackSpeed;
        AttackRange = stats.AttackRange;
    }
    public void AddSkill(SkillDataSO data)
    {
        var proj = data.CreateProjectileStrategy();
        if (proj != null)
        {
            mArrowStrategies.Add(proj);
        }

        var passive = data.CreatePassiveStrategy();
        if (passive != null)
        {
            passive.OnEquip(this);
            mPassiveStrategies.Add(passive);
        }
    }

    public void MakeProjectile()
    {
        Projectile projectile = Managers.Pool.GetFromPool(mProjectilePrefab);
        if (projectile != null)
        {
            projectile.transform.position = transform.position + mProjectileOffeset;
            projectile.Setup(mArrowStrategies, AttackDamage);
        }
    }
}
