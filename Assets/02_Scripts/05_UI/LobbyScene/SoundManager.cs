using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //사운드매니저 싱글톤으로 만들어서 메인로비,스테이지에서 사용하기
    public static SoundManager Instance { get; private set; }

    public AudioSource mBgmSound;
    public AudioSource mSfxSound;

    public string mLobbySceneName = "Lobby_JJH";
    public AudioClip mLobbybgm;
    public string mStageSceneName= "Stage_Temp";
    public AudioClip mStagebgm;

    [Header("파일을 직접연결하세요")]
    public AudioClip mPlayerAttackSound;
    public AudioClip mPlayerHitSound;
    public AudioClip mMonsterDieSound;
    public AudioClip mCoinSound;
    public AudioClip mBtnSound;
    //스킬이펙트
    //슬롯머신돌아가는소리
    //아이템장착
    //아이템 팔때
    //죽을때
    //클리어할때


   
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        float bgm = PlayerPrefs.GetFloat("Bgm", 0.5f);
        float sfx = PlayerPrefs.GetFloat("Sfx", 0.5f);

        SetBgmVolume(bgm);
        SetSfxVolume(sfx);
    }
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene.name== mLobbySceneName)
        {
            BgmChange(mLobbybgm);
        }
        else if (scene.name == mStageSceneName)
        {
            BgmChange(mStagebgm);
        }
    }
    private void BgmChange(AudioClip clip)
    {
        if (clip == null || mBgmSound.clip == clip) return;
        mBgmSound.clip = clip;
        mBgmSound.Play();
    }
    public void BtnSound()
    {
        if (mSfxSound != null)
        {
            mSfxSound.Play();
        }
    }

    //이게 효과음 재생하는 함수
    //오디오 클립 변수를 입력하세요
    //SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mPlayerAttackSound);
    public void PlaySfxSound(AudioClip clip)
    {        
        mSfxSound.PlayOneShot(clip);
    }



    //소리 조절기능
    public void SetBgmVolume(float volume)
    {
        if (mBgmSound != null)
        {
            mBgmSound.volume = volume;
        }
    }
    public void SetSfxVolume(float volume)
    {
        mSfxSound.volume= volume;
        //if (mBtnSound != null)
        //{
        //    mBtnSound.volume = volume;
        //}
        //if (mAttackSound != null)
        //{
        //    mAttackSound.volume = volume;
        //}
    }
}
