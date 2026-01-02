using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image mIcon;
    [SerializeField] private TextMeshProUGUI mNameText;
    [SerializeField] private TextMeshProUGUI mDescText;
    [SerializeField] private Button mButton;
    [SerializeField] private Image mBtnImage;

    [Header("Grade별 Slot")]
    [SerializeField] private Sprite mSlotLegend;
    [SerializeField] private Sprite mSlotEpic;
    [SerializeField] private Sprite mSlotExpert;
    [SerializeField] private Sprite mSlotNormal;

    private readonly Vector3 mSlotRectOffsetLegend = new Vector3(193, 450, 4);
    private readonly Vector3 mSlotRectOffsetEpic = new Vector3(203, 467, 5);
    private readonly Vector3 mSlotRectOffsetExpert = new Vector3(180, 440, 0);
    private readonly Vector3 mSlotRectOffsetNormal = new Vector3(180, 440, 0);
    private SkillDataSO mData;
    private event Action<SkillDataSO> mOnSelectedSkill;

    // 초기화 함수
    public void Setup(SkillDataSO data, Action<SkillDataSO> onSelected)
    {
        mData = data;
        mOnSelectedSkill = onSelected;

        // UI 갱신
        if (data != null)
        {
            mIcon.sprite = data.icon;
            mNameText.text = data.skillName;
            mDescText.text = data.description;
            ChangeBtnRectAndImage(data.skillGrade);
        }

        // 버튼 리스너 초기화 (중복 방지)
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        // 클릭되면 등록된 콜백(부모의 함수)을 실행하면서 내 데이터를 넘겨줌
        mOnSelectedSkill?.Invoke(mData);
    }
    private void ChangeBtnRectAndImage(ESkillGrade grade)
    {
        switch (grade)
        {
            case ESkillGrade.None:
            case ESkillGrade.Normal:
                CalGradeImageRect(mSlotNormal, mSlotRectOffsetNormal);
                break;
            case ESkillGrade.Expert:
                CalGradeImageRect(mSlotExpert, mSlotRectOffsetExpert);
                break;
            case ESkillGrade.Epic:
                CalGradeImageRect(mSlotEpic, mSlotRectOffsetEpic);
                break;
            case ESkillGrade.Legend:
                CalGradeImageRect(mSlotLegend, mSlotRectOffsetLegend);
                break;
            default:
                CalGradeImageRect(mSlotNormal, mSlotRectOffsetNormal);
                break;
        }
    }
    private void CalGradeImageRect(Sprite grade, Vector3 offset)
    {
        mBtnImage.sprite = grade;
        mBtnImage.rectTransform.sizeDelta = offset;
        Vector2 pos = mBtnImage.rectTransform.anchoredPosition;
        pos.y = offset.z;
        mBtnImage.rectTransform.anchoredPosition = pos;
    }
}
