using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image mIconImage;
    private SkillDataSO mSkillDataSO;
    [SerializeField] private Button mButton;

    private void Awake()
    {
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(ShowSlot);
        mButton.onClick.AddListener(PlayBtnSound);
    }
    public void Setup(SkillDataSO dataSO)
    {
        mSkillDataSO = dataSO;
        //mIconImage.sprite = dataSO.icon;

        if (mIconImage != null && dataSO != null)
        {
            mIconImage.sprite = dataSO.icon;
        }

    }
    public void ShowSlot()
    {
        if (mSkillDataSO != null)
        {
            SkillText.Instance.Show(mSkillDataSO);
        }
        //SkillText.Instance.Show(mSkillDataSO);
    }
    public void PlayBtnSound()
    {
        SoundManager.Instance.BtnSound();

    }
}
