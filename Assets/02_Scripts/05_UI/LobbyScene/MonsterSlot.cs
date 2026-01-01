using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    public Image iconImage;
    private MonsterData monsterData;
    [SerializeField] private Button mButton;

    private void Awake()
    {
        mButton.onClick.RemoveListener(PlayBtnSound);
        mButton.onClick.AddListener(PlayBtnSound);
    }

    public void Setup(MonsterData data)
    {
        monsterData = data;
        iconImage.sprite = data.MonsterIcon;
    }
    public void ShowSlot()
    {
        SkillText.Instance.ShowMonsterList(monsterData);
    }
    private void PlayBtnSound()
    {
        SoundManager.Instance.BtnSound();
    }
}
