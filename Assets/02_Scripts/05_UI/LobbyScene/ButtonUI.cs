using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] GameObject[] mButton;
    public string mURL = "https://github.com/game-master-m/Oz_TeamProject_Archero.git";
    public GameObject mTeamName;
    public GameObject mShop;
    public GameObject mEquip;
    public GameObject mOption;
    public GameObject mRanking;
    public GameObject mSkillList;
    public GameObject mMonsterList;
    public GameObject mSoundPanel;

    public void ImageOn()
    {
        foreach (var img in mButton)
        {
            if (img != null)
            {
                img.SetActive(!img.activeSelf);
            }
        }
    }
    public void OpenLink()
    {
        Application.OpenURL(mURL);
    }
    public void ShowName()
    {
        mTeamName.SetActive(!mTeamName.activeSelf);
        SoundManager.Instance.BtnSound();
    }
    public void ShowShop()
    {
        if (!mShop.activeSelf)
        {
            mEquip.SetActive(false);
        }
        mShop.SetActive(!mShop.activeSelf);
        SoundManager.Instance.BtnSound();
    }
    public void ShowEquip()
    {
        if (!mEquip.activeSelf)
        {
            mShop.SetActive(false);
        }
        mEquip.SetActive(!mEquip.activeSelf);
        SoundManager.Instance.BtnSound();
    }
    public void ShowOption()
    {
        mShop.SetActive(false);
        mEquip.SetActive(false);

        mOption.SetActive(true);

        SoundManager.Instance.BtnSound();
    }
    public void CloseOption()
    {
        mOption.SetActive(false);
        mRanking.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ShowRanking()
    {
        mRanking.SetActive(true);
        SoundManager.Instance.BtnSound();
    }
    public void ShowSkillList()
    {
        mSkillList.SetActive(true);
        SoundManager.Instance.BtnSound();
    }
    public void CloseSkillList()
    {
        mSkillList.SetActive(false);
    }
    public void ShowMonsterList()
    {
        mMonsterList.SetActive(true);
        SoundManager.Instance.BtnSound();
    }
    public void CloseMonsterList()
    {
        mMonsterList.SetActive(false);
    }
    public void MainLobby()
    {
        mShop.SetActive(false);
        mEquip.SetActive(false);
        SoundManager.Instance.BtnSound();
    }
    public void OpenSoundPanel()
    {
        mSoundPanel.SetActive(true);
        SoundManager.Instance.BtnSound();
    }
    public void CloseSoundPanel()
    {
        mSoundPanel.SetActive(false);
    }

    public void PlayBtnSound()
    {
        SoundManager.Instance.BtnSound();
    }
}
