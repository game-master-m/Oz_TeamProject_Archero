using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //사운드매니저 싱글톤으로 만들어서 메인로비,스테이지에서 사용하기
    public static SoundManager Instance { get; private set; }

    public AudioSource mBgmSound;
    public AudioSource mSfxSound;

    public string mLobbySceneName = "Lobby_Temp";
    public AudioClip mLobbybgm;
    public string mStageSceneName = "Stage_Temp";
    public AudioClip mStagebgm;

    [Header("파일을 직접연결하세요")]
    public AudioClip mPlayerAttackSound;
    public AudioClip mPlayerHitSound;
    public AudioClip mMonsterDieSound;
    public AudioClip mGetCoinSound;
    public AudioClip mBtnSound;
    public AudioClip mGameOverSound;
    public AudioClip mGameClearSound;
    public AudioClip mSellItemSound;
    public AudioClip mEquipSound;
    public AudioClip mLevelUpSound;
    public AudioClip mMonsterHitSound;
    public AudioClip mSlotRotationSound;
    public AudioClip mSlotSeletSound;
    public AudioClip mGetExpSound;
    public AudioClip mUpgradeFlashSound_Normal;
    public AudioClip mUpgradeFlashSound_Expert;
    public AudioClip mUpgradeFlashSound_Epic;

    private Dictionary<AudioClip, float> mPlayingSfxTracks = new Dictionary<AudioClip, float>();
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mLobbySceneName)
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

    //효과음 재생 메서드
    //오디오 클립 변수를 입력하세요
    //SoundManager.Instance.PlaySfxSound(SoundManager.Instance.mPlayerAttackSound);
    public void PlaySfxSound(AudioClip clip)
    {
        mSfxSound.PlayOneShot(clip);
    }
    public void PlaySfxUnique(AudioClip clip)
    {
        if (clip == null) return;

        if (mPlayingSfxTracks.ContainsKey(clip))
        {
            if (Time.time < mPlayingSfxTracks[clip])
            {
                return;
            }
        }
        PlaySfxSound(clip);
        mPlayingSfxTracks[clip] = Time.time + clip.length;
    }
    //효과음 끌때
    public void StopSfxSound()
    {
        mSfxSound.Stop();
        mPlayingSfxTracks.Clear();
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
        mSfxSound.volume = volume;
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
