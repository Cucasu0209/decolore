
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Lofelt.NiceVibrations;
using Lean.Pool;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        Instance = this;
        SetBGMusicState(UserData.GetMusicState());
        SetSoundFxState(UserData.GetSoundFxState());
        SetHapticState(UserData.GetHapticState());
    }
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


    #region LOAD BG MUSIC
    private void Start()
    {
        UIManager.Instance.OnLoadingAOAComplete += SwitchToMainMenuBGM;

    }
    #endregion LOAD BG MUSIC

    #region Background Music
    [SerializeField] private AudioSource BGAudioSource;
    [SerializeField] private AudioClip Background_MainMenu;

    public float BGMusicVolume { get; private set; } = 1;
    public void SetBGMusicState(bool turnOn)
    {
        UserData.SetMusicState(turnOn);
        BGMusicVolume = turnOn ? 0.6f : 0;
        BGAudioSource.volume = BGMusicVolume;

    }


    public void SwitchToMainMenuBGM()
    {
        if (BGAudioSource.clip != Background_MainMenu)
        {
            if (BGAudioSource.clip != Background_MainMenu)
                BGAudioSource.clip = Background_MainMenu;
            BGAudioSource.mute = false;
            if (BGAudioSource.isPlaying == false)
                BGAudioSource.Play();
        }
        else
        {
            BGAudioSource.mute = false;
            BGAudioSource.UnPause();
        }

    }
    public void StopBGMusic()
    {
        BGAudioSource.mute = true;
    }
    #endregion Background Music

    #region SOUND EFFECT
    [SerializeField] private AudioSource SFXAudioSource;
    public float SFXVolume { get; private set; } = 1;

    public void SetSoundFxState(bool turnOn)
    {
        UserData.SetSoundFxState(turnOn);
        SFXVolume = turnOn ? 1 : 0;
        SFXAudioSource.volume = SFXVolume;

        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            CurrentLoopSounds[i].volume = SFXVolume;
        }
    }

    private Dictionary<string, Vector2> SoundDelayTimeDict = new Dictionary<string, Vector2>();
    private Dictionary<string, float> CurrentSoundPlayed = new Dictionary<string, float>();
    public void PlayEffect(AudioClip clip, float pitch = 1, float volume = 1, float delayTime = -1, string soundName = "")
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }
        SFXAudioSource.pitch = pitch;
        if (delayTime < 0)
        {
            SFXAudioSource.PlayOneShot(clip, volume);

            //if (CurrentSoundPlayed.ContainsKey(clip.name) == false)
            //{
            //    CurrentSoundPlayed.Add(clip.name, Time.realtimeSinceStartup);
            //    SFXAudioSource.PlayOneShot(clip, volume);

            //}
            //else if (CurrentSoundPlayed[clip.name] - Time.realtimeSinceStartup > 0.1f)
            //{
            //    CurrentSoundPlayed[clip.name] = Time.realtimeSinceStartup;
            //    SFXAudioSource.PlayOneShot(clip, volume);
            //}
        }
        else
        {
            if (soundName == "")
            {
                Debug.LogError("Thiếu sound name cho sound cần delay");
            }
            if (SoundDelayTimeDict.ContainsKey(soundName) == false)
            {
                SoundDelayTimeDict.Add(soundName, Vector2.zero);
            }
            if (Time.realtimeSinceStartup >= SoundDelayTimeDict[soundName].x + SoundDelayTimeDict[soundName].y)
            {
                SoundDelayTimeDict[soundName] = new Vector2(Time.realtimeSinceStartup, delayTime);
                SFXAudioSource.PlayOneShot(clip, volume);
            }
        }

    }


    public void PlayEffectWithDelay(AudioClip clip, Action callbackAction = null, float delayTime = 0.25f, float pitch = 1, float volume = 1)
    {
        if (callbackAction != null)
        {
            DOVirtual.DelayedCall(delayTime, () => callbackAction());
        }
        PlayEffect(clip, pitch, volume);
    }
    #endregion SOUND EFFECT

    #region MUTUAL SOUND
    [SerializeField] private AudioClip ButtonSound;

    public void PlayButtonSound()
    {
        Vibrate(1);
        PlayEffect(ButtonSound);
    }
    #endregion MUTUAL SOUND

    #region LOOP SOUND
    private List<AudioSource> CurrentLoopSounds = new List<AudioSource>();
    public void PlayLoop(AudioClip clip, float pitch = 1, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }

        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            if (CurrentLoopSounds[i].loop == true && CurrentLoopSounds[i].clip == clip)
            {
                return;
            }
        }
        AudioSource newSource = LeanPool.Spawn(SFXAudioSource.gameObject, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        newSource.transform.SetParent(transform);

        CurrentLoopSounds.Add(newSource);
        newSource.pitch = pitch;
        newSource.volume = SFXVolume > 0 ? volume : SFXVolume;
        newSource.clip = clip;
        newSource.loop = true;
        newSource.Play();
    }
    public void StopLoopSound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }

        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            if (CurrentLoopSounds[i].loop == true && CurrentLoopSounds[i].clip == clip)
            {
                CurrentLoopSounds[i].Stop();
                LeanPool.Despawn(CurrentLoopSounds[i].gameObject);
                CurrentLoopSounds.RemoveAt(i);
                return;
            }
        }
    }

    //not destroy
    public void PlayAllLoopingSound()
    {
        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            CurrentLoopSounds[i].Play();
        }
    }
    //not destroy
    public void StopAllLoopingSound()
    {
        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            CurrentLoopSounds[i].Stop();
        }

    }
    #endregion

    #region Haptic
    public bool HapticOn { get; private set; }
    public void SetHapticState(bool turnOn)
    {
        UserData.SetHapticState(turnOn);
        HapticOn = turnOn;
    }
    /// <summary>
    /// Bật tắt rung
    /// </summary>
    private float HapticDelay = 0.5f;
    private float HapticLastTime = 0;
    public void Vibrate(int strength)
    {
        HapticOn = UserData.GetHapticState();
        if (HapticOn == false) return;
        //Trung Comment
        switch (strength)
        {
            case 0: // OFF
                break;
            case 1: // LIGHT
                if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
                {
                    HapticLastTime = Time.realtimeSinceStartup;
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                }
                break;
            case 2: // MEDIUM
                if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
                {
                    HapticLastTime = Time.realtimeSinceStartup;
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                }
                break;
            case 3: // HEAVY
                if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
                {
                    HapticLastTime = Time.realtimeSinceStartup;
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                }
                break;
            default:
                break;
        }
    }
    #endregion Haptic
}
