using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mAudioMixer;
    [SerializeField] private Slider mMasterSlider;
    [SerializeField] private Slider mBgmSlider;
    [SerializeField] private Slider mSfxSlider;

    private void Awake()
    {
        mMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        mBgmSlider.onValueChanged.AddListener(SetBgmVolume);
        mSfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }
    public void SetMasterVolume(float volume)
    {
        mAudioMixer.SetFloat("Master", Mathf.Log10(volume)*20);
    }
    public void SetBgmVolume(float volume)
    {
        mAudioMixer.SetFloat("Bgm", Mathf.Log10(volume) * 20);
    }
    public void SetSfxVolume(float volume)
    {
        mAudioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
    }
}
