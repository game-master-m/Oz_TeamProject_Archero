using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillListUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillDataSO mSkillData;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mSkillData != null)
        {
            SkillText.Instance.Show(mSkillData);
        }


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillText.Instance.Out();
    }
}
