using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [Header("경험치")]
    [SerializeField] private float mRequiredExp = 100; // 필요 경험치 (임시)

    [Header("이벤트 구독")]
    [SerializeField] private IntEventChannelSO mOnGetExpRequest; // 각 경험치들이 발송



    private int mCurrentExp = 0;
    private float mExpMultiplier = 1.2f;
    private void OnEnable()
    {
        mOnGetExpRequest.onEvent += HandleGetExp;
    }
    private void OnDisable()
    {
        mOnGetExpRequest.onEvent -= HandleGetExp;
    }

    private void HandleGetExp(int exp)
    {
        mCurrentExp += exp;
        Utils.Log($"경험치 획득! 현재 경험치: {mCurrentExp} / {mRequiredExp}");
        if (mCurrentExp >= mRequiredExp)
        {
            mCurrentExp = 0;
            mRequiredExp *= mExpMultiplier; // 필요 경험치 증가
            //StageManger 에게 레벨업 요청
            Managers.Stage.LevelUp();
        }
    }


}
