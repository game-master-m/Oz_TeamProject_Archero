using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Projectile mProjectilePrefab;
    [SerializeField] private Vector3 mProjectileOffeset = new Vector3(0, 1.0f, 0);

    private List<IProjectileStrategy> mArrowStrategies = new List<IProjectileStrategy>();
    private List<IPassiveStrategy> mPassiveStrategies = new List<IPassiveStrategy>();

    //모든 스탯은 PlayerStat이 들고있고 관리 함
    private PlayerStat mStat;

    public bool IsAutoTurret { get; set; } = false;
    public PlayerStat Stat => mStat;
    private void Start()
    {
        Managers.Pool.CreatePool(mProjectilePrefab, 100, Managers.Pool.transform);

        mArrowStrategies.Add(new BasicArrowStrategy());
    }
    private void OnDisable()
    {
        foreach (var strategy in mPassiveStrategies)
        {
            strategy?.OnUnequip(this);
        }
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
        mStat = stats;
    }
    public void AddSkill(SkillDataSO data)
    {
        var proj = data.CreateProjectileStrategy();
        if (proj != null)
        {
            //true -> Add , false -> Stack
            AddOrStack(mArrowStrategies, proj);
            //mArrowStrategies.Add(proj);
        }

        var passive = data.CreatePassiveStrategy();
        if (passive != null)
        {
            //true -> Add , false -> Stack
            passive.OnEquip(this);
            AddOrStack(mPassiveStrategies, passive);

            //passive.OnEquip(this);
            //mPassiveStrategies.Add(passive);
        }
    }


    public Projectile MakeProjectile(Transform firstTarget)
    {
        Projectile projectile = Managers.Pool.GetFromPool(mProjectilePrefab);
        if (projectile != null)
        {
            projectile.transform.position = transform.position + mProjectileOffeset;
            projectile.Setup(mArrowStrategies, mStat.AttackDamage, mStat.AttackRange, firstTarget);

            //SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mPlayerAttackSound);
        }
        return projectile;
    }
    public Projectile MakeProjectile()
    {
        return MakeProjectile(null);
    }


    //리턴이 true면 Add가 된 것이고 , false이면 Stack이 쌓임
    private bool AddOrStack<T>(List<T> list, T newStrategy) where T : class
    {
        // 리스트를 순회하며 "나랑 합칠 수 있는 녀석"을 찾습니다.
        foreach (T existing in list)
        {
            // 1. 기존 전략이 IStackable<T>를 구현했는지 확인
            if (existing is ISkillStackable<T> stackable)
            {
                // 2. 구현했다면 합치기 시도 (TryStack 내부에서 타입 체크 수행) (타입이 안 맞으면 if문 false)
                if (stackable.TryStack(newStrategy))
                {
                    // 합치기 성공! 리스트에 추가 안 함.
                    return false;
                }
            }
        }

        // 합칠 상대를 못 찾았으면 리스트에 새롭게 추가
        list.Add(newStrategy);
        return true;
    }
}
