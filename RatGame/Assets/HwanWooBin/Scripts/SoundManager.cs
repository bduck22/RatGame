using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float Mastervalue;
    public bool IsMasterMute;

    public AudioClip[] Bgms;
    public float Bgmvalue;
    public bool IsBgmMute;

    public AudioClip[] Sfxs;
    public float Sfxvalue;
    public bool IsSfxMute;

    public static SoundManager instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}
