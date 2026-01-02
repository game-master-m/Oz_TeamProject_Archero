using System;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [Header("경험치")]
    [SerializeField] private float mRequiredExp = 100; // 필요 경험치 (임시)

    [Header("이벤트 구독")]
    [SerializeField] private IntEventChannelSO mOnGetExpRequest; // 각 경험치들이 발송
    [SerializeField] private VoidEventChannelSO mOnNoticeLastRoom; //StageManager.cs가 발송(마지막 룸)

    //경험치 프로그레스 바 관련
    public event Action<float> onExpChange; //ExpProgressController 구독
    public event Action<int> onLevelChange; //ExpProgressController 구독
    public event Action<int> onGoldChange;  //ExpProgressController 구독

    private int mCurrentExp = 0;
    private int mCurrentLev = 1;
    private float mExpMultiplier = 1.2f;

    private bool bIsLastRoom = false;
    private void Start()
    {
        onExpChange?.Invoke(0);
        onGoldChange?.Invoke(0);
        onLevelChange?.Invoke(1);
    }
    private void OnEnable()
    {
        mOnGetExpRequest.onEvent += HandleGetExp;
        mOnNoticeLastRoom.onEvent += HandleLastRoom;
        mCurrentLev = 1;
        bIsLastRoom = false;
    }
    private void OnDisable()
    {
        mOnGetExpRequest.onEvent -= HandleGetExp;
        mOnNoticeLastRoom.onEvent -= HandleLastRoom;

        bIsLastRoom = false;
    }
    private void HandleLastRoom()
    {
        bIsLastRoom = true;
    }
    private void HandleGetExp(int exp)
    {
        mCurrentExp += exp;
        Utils.Log($"경험치 획득! 현재 경험치: {mCurrentExp} / {mRequiredExp}");

        //여기서 비율 전달
        onExpChange?.Invoke(mCurrentExp / mRequiredExp);
        //골드는 획득한 경험치 갯수 기준 골드수급(경험치 프리팹 하나당 3?)
        onGoldChange?.Invoke(Define.GetGoldAmountPerExp);

        if (mCurrentExp >= mRequiredExp)
        {
            mCurrentExp = 0;
            mRequiredExp *= mExpMultiplier; // 필요 경험치 증가
            mCurrentLev++;
            //레벨업 사운드
            SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mLevelUpSound);

            //여기서 전투씬 레벨 전달
            onLevelChange?.Invoke(mCurrentLev);

            //StageManger 에게 레벨업 요청
            if (!bIsLastRoom)
            {
                Managers.Stage.LevelUp();
                Managers.Game.CanPause = false;
            }
        }
    }


}
