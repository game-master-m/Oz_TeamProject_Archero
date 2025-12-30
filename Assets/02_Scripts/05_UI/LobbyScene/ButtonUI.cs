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
        foreach(var img in mButton)
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

    }
    public void ShowShop()
    {
        if (!mShop.activeSelf)
        {
            mEquip.SetActive(false);
        }
        mShop.SetActive(!mShop.activeSelf);
    }
    public void ShowEquip()
    {
        if (!mEquip.activeSelf)
        {
            mShop.SetActive(false);
        }
        mEquip.SetActive(!mEquip.activeSelf);
    }
    public void ShowOption()
    {
        mShop.SetActive(false);
        mEquip.SetActive(false);

        mOption.SetActive(true);
    }
    public void CloseOption()
    {
        mOption.SetActive(false);
        mRanking.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        Utils.Log("게임종료");
    }
    public void ShowRanking()
    {
        mRanking.SetActive(true);
    }
    public void ShowSkillList()
    {
        mSkillList.SetActive(true);
    }
    public void CloseSkillList()
    {
        mSkillList.SetActive(false);
    }
    public void ShowMonsterList()
    {
        mMonsterList.SetActive(true);
    }
    public void CloseMonsterList()
    {
        mMonsterList.SetActive(false);
    }
    public void MainLobby()
    {
        mShop.SetActive(false);
        mEquip.SetActive(false);
    }
    public void OpenSoundPanel()
    {
        mSoundPanel.SetActive(true);
    }
    public void CloseSoundPanel()
    {
        mSoundPanel.SetActive(false);
    }
    
   
}
