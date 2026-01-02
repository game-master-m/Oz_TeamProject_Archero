using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillText : MonoBehaviour
{
    public static SkillText Instance;

    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI desText;
    public Image icon;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        
    }
    public void Show(SkillDataSO skillDataSO)
    {
        
        nameText.text = skillDataSO.skillName;
        desText.text = skillDataSO.description;
        icon.sprite = skillDataSO.icon;
        gameObject.SetActive (true);
    }
    public void ShowMonsterList(MonsterData monsterData)
    {
        nameText.text = monsterData.MonsterName;
        desText.text = monsterData.MonsterDescription;
        icon.sprite = monsterData.MonsterIcon;
        gameObject.SetActive(true);
    }
    public void Out()
    {
        gameObject.SetActive(false);
    }
}
