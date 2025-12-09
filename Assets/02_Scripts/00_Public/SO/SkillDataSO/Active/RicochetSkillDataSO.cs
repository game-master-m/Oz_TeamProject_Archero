
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet_Lv", menuName = "Archero/SkillData/Active/RicochetSkillDataSO")]
public class RicochetSkillDataSO : SkillDataSO
{
    [Header("Ricochet 능력치")]
    [SerializeField] private int mMaxBounceCount = 3;
    [SerializeField] private float mBounceRange = 10.0f;
    [SerializeField] private float mDamageMultiplier = 0.8f;

    // 팩토리 메서드 구현: 여기서 실제 전략 객체(인스턴스)를 생성해서 반환
    public override IProjectileStrategy CreateProjectileStrategy()
    {
        return new RicochetStrategy(mMaxBounceCount, mBounceRange, mDamageMultiplier);
    }
}
