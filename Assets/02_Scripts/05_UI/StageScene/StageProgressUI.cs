using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageProgressUI : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] private TextMeshProUGUI mPrevRoomNumText;
    [SerializeField] private TextMeshProUGUI mCurrentRoomNumText;
    [SerializeField] private TextMeshProUGUI mNextRoomNumText;
    [SerializeField] private TextMeshProUGUI mLastRoomNumText;

    [SerializeField] private Image[] mRoomImages;
    [SerializeField] private Image[] mBattleImages;

    [SerializeField] private Image mDotImage;
    [SerializeField] private Image mLeftArrowImage;
    [SerializeField] private Image mRightArrowImage;

    [SerializeField] private Sprite mBattleIcon;
    [SerializeField] private Sprite mBattleFrame;
    [SerializeField] private Sprite mBossIcon;
    [SerializeField] private Sprite mBossFrame;

    [Header("이벤트 구독")]
    [SerializeField] private IntListEventChannelSO mOnStageProgressStart;     //StageManager가 발송
    [SerializeField] private IntEventChannelSO mOnRoomNumChange;              //StageManager가 발송

    private int mTotalRoomCount;
    private List<int> mBossRoomNumList;
    private int mNextBossRoomNum = 0;
    private int mBossRoomIndex = 0;
    private void Awake()
    {
        mNextBossRoomNum = 0;
        mBossRoomIndex = 0;
    }
    private void OnEnable()
    {
        mOnStageProgressStart.onEvent += SetUpStageProgress;
        mOnRoomNumChange.onEvent += ShowProgressUI;

    }
    private void OnDisable()
    {
        mOnStageProgressStart.onEvent -= SetUpStageProgress;
        mOnRoomNumChange.onEvent -= ShowProgressUI;
    }
    private void SetUpStageProgress(int roomCount, List<int> bossRoomNum)
    {
        mTotalRoomCount = roomCount;
        mBossRoomNumList = new List<int>(bossRoomNum);
    }
    private void ShowProgressUI(int currentRoomIndex)
    {

        int currentRoomNum = currentRoomIndex + 1;

        int prevNum = currentRoomNum - 1;
        int nextRoomNum = currentRoomNum + 1;

        mPrevRoomNumText.SetText(Utils.IntAppend(prevNum));
        mCurrentRoomNumText.SetText(Utils.IntAppend(currentRoomNum));
        mNextRoomNumText.SetText(Utils.IntAppend(nextRoomNum));

        if (nextRoomNum == mBossRoomNumList[mBossRoomIndex] && nextRoomNum < mBossRoomNumList[mBossRoomNumList.Count - 1])
        {
            mBossRoomIndex++;
        }
        mNextBossRoomNum = mBossRoomNumList[mBossRoomIndex];
        mLastRoomNumText.SetText(Utils.IntSlashInt(mNextBossRoomNum, mTotalRoomCount));

        for (int j = 0; j < mRoomImages.Length - 1; j++)
        {
            mRoomImages[j].sprite = mBattleFrame;
            mBattleImages[j].sprite = mBattleIcon;
        }
        mRoomImages[3].sprite = mBossFrame;
        mBattleImages[3].sprite = mBossIcon;

        foreach (var bossRoomNum in mBossRoomNumList)
        {
            Utils.Log($"보스 룸 넘버 : {bossRoomNum}");
            if (prevNum == bossRoomNum)
            {
                mRoomImages[0].sprite = mBossFrame;
                mBattleImages[0].sprite = mBossIcon;
            }
            if (currentRoomNum == bossRoomNum)
            {
                mRoomImages[1].sprite = mBossFrame;
                mBattleImages[1].sprite = mBossIcon;
            }
            if (nextRoomNum == bossRoomNum)
            {
                mRoomImages[2].sprite = mBossFrame;
                mBattleImages[2].sprite = mBossIcon;
            }
        }

        if (prevNum <= 0)
        {
            mRoomImages[0].gameObject.SetActive(false);
            mLeftArrowImage.enabled = false;
        }
        else
        {
            mRoomImages[0].gameObject.SetActive(true);
            mLeftArrowImage.enabled = true;
        }

        if (nextRoomNum >= mTotalRoomCount)
        {
            mRoomImages[3].gameObject.SetActive(false);
            mDotImage.enabled = false;
        }
        else
        {
            mRoomImages[3].gameObject.SetActive(true);
            mDotImage.enabled = true;
        }

        if (currentRoomNum >= mTotalRoomCount)
        {
            mRoomImages[2].gameObject.SetActive(false);
            mRightArrowImage.enabled = false;
        }
        else
        {
            mRoomImages[2].gameObject.SetActive(true);
            mRightArrowImage.enabled = true;
        }

    }



}
