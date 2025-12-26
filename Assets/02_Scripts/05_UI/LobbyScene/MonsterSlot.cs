using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    public Image iconImage;
    private MonsterData monsterData;
    private Button button;

    public void Setup(MonsterData data)
    {
        monsterData = data;
        iconImage.sprite = data.MonsterIcon;
    }
    public void ShowSlot()
    {
        SkillText.Instance.ShowMonsterList(monsterData);
    }
}
