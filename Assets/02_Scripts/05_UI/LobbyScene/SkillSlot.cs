using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour
{
    public Image mIconImage;
    private SkillDataSO mSkillDataSO;
    private Button mButton;

    private void Awake()
    {
        mButton = GetComponent<Button>();
        if (mButton != null)
        {
            mButton.onClick.AddListener(ShowSlot);
        }
    }
    public void Setup(SkillDataSO dataSO)
    {
        mSkillDataSO = dataSO;
        //mIconImage.sprite = dataSO.icon;

        if (mIconImage != null && dataSO != null)
        {
            mIconImage.sprite=dataSO.icon;
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
   
}
