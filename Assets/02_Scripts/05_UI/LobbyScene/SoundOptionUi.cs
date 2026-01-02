using UnityEngine;
using UnityEngine.UI;

public class SoundOptionUi : MonoBehaviour
{
    public Slider mBgmSlider;
    public Slider mSfxSlider;

    private void OnEnable()
    {
        float saveBgm = PlayerPrefs.GetFloat("Bgm", 0.5f);
        float saveSfx = PlayerPrefs.GetFloat("Sfx", 0.5f);

        mBgmSlider.value = saveBgm;
        mSfxSlider.value = saveSfx;

        mBgmSlider.onValueChanged.RemoveAllListeners();
        mSfxSlider.onValueChanged.RemoveAllListeners();

        mBgmSlider.onValueChanged.AddListener(BgmSliderChange);
        mSfxSlider.onValueChanged.AddListener(SfxSliderChange);
                
    }
    public void BgmSliderChange(float volume)
    {
        SoundManager.Instance.SetBgmVolume(volume);
        PlayerPrefs.SetFloat("Bgm", volume);
    }
    public void SfxSliderChange(float volume)
    {
        SoundManager.Instance.SetSfxVolume(volume);
        PlayerPrefs.SetFloat("Sfx", volume);
    }
}
