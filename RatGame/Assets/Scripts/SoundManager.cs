using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider MasterSlider;
    public float Mastervalue;
    public bool IsMasterMute;
    public Image MasterMuteImage;

    public AudioClip[] Sfxs;
    public Slider SfxSlider;
    public float Sfxvalue;
    public bool IsSfxMute;
    public Image SfxMuteImage;

    public AudioClip[] Bgms;
    public Slider BgmSlider;
    public float Bgmvalue;
    public bool IsBgmMute;
    public Image BgmMuteImage;

    public static SoundManager instance;

    public AudioSource BgmSource;
    public AudioSource SfxSource;

    void Start()
    {
        instance = this;
    }

    public void OnOffMuteSound(int type)
    {
        switch (type)
        {
            case 0:IsMasterMute = !IsMasterMute; break;
            case 1:IsSfxMute = !IsSfxMute; break;
            case 2:IsBgmMute = !IsBgmMute; break;
        }

        MasterMuteImage.gameObject.SetActive(!IsMasterMute);
        SfxMuteImage.gameObject.SetActive(!IsSfxMute);
        BgmMuteImage.gameObject.SetActive(!IsBgmMute);
        loadSound();
    }

    public void SetSound(int type)
    {
        Mastervalue = MasterSlider.value;
        Sfxvalue = SfxSlider.value;
        Bgmvalue = BgmSlider.value;

        loadSound();
    }

    public void loadSound()
    {
        audioMixer.SetFloat("MASTER", (IsMasterMute?-40:Mastervalue));
        audioMixer.SetFloat("SFX", (IsSfxMute ? -40 : Sfxvalue));
        audioMixer.SetFloat("BGM", (IsBgmMute ? -40 : Bgmvalue));

    }
}
