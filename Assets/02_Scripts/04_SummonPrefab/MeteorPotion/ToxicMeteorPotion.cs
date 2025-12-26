
using UnityEngine;

public class ToxicMeteorPotion : PotionBase
{
    private ToxicMeteorPotionSkillDataSO mSkillDataSO;
    private PlayerAttack mPlayer;
    private ToxicMeteor mMeteorPrefab;

    private Vector3 mMeteorOffset;

    private ToxicMeteor mToxicMeteor;

    //세팅
    public void SetUp(ToxicMeteorPotionSkillDataSO skillDataSO, PlayerAttack attack)
    {
        mSkillDataSO = skillDataSO;
        mPlayer = attack;
        mMeteorPrefab = skillDataSO.MeteorPrefab;
        mMeteorOffset = skillDataSO.MeteorOffset;
        Managers.Pool.CreatePool(mMeteorPrefab, 1, Managers.Pool.transform);
        SetPosition(attack);
    }

    public override void ApplyPotionEffect()
    {
        if (FindCloseEnemy() != null)
        {
            Vector3 targetPos = FindCloseEnemy().position;
            //메테오 생성
            mToxicMeteor = Managers.Pool.GetFromPool(mMeteorPrefab);
            mToxicMeteor.gameObject.transform.position = targetPos + mMeteorOffset;
            mToxicMeteor.SetUp(mSkillDataSO, mPlayer);
        }
        Managers.Pool.ReturnToPool(this);
    }
}
