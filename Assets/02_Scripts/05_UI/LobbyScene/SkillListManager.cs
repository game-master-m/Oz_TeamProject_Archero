using System.Collections.Generic;
using UnityEngine;

public class SkillListManager : MonoBehaviour
{
    public SkillContainerSO mSkillDataList;
    public GameObject mSlotPrefab;
    public Transform mBox;

    private void Start()
    {
        gameObject.SetActive(false);
        foreach(SkillDataSO data in mSkillDataList.AllSkills)
        {
            GameObject go = Instantiate(mSlotPrefab, mBox);
            go.GetComponent<SkillSlot>().Setup(data);
        }
        
        SkillSlot[] slots=GetComponentsInChildren<SkillSlot>();

        for(int i = 0; i < slots.Length; i++)
        {
            if (i < mSkillDataList.AllSkills.Count)
            {
                slots[i].Setup(mSkillDataList.AllSkills[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}
