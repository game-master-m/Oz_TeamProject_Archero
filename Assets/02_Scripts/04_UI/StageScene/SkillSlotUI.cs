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

    private SkillDataSO mData;
    private Action<SkillDataSO> mOnSelectedSkill;

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
}
