using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


/// <summary>
/// Manages the audio to be played in the game
/// </summary>

public class AudioManager : MonoBehaviour
{
    #region Variables
    [SerializeField] Sound[] _sounds;
    String[] _footSteps;
    #endregion

    #region Singleton Pattern
    public static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();

                if (_instance == null) _instance = new GameObject().AddComponent<AudioManager>();
            }

            return _instance;

        }
    }
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        SetUpAudioSources();
    }
    #endregion

    #region PrivateMethods
    void SetUpAudioSources()
    {
        foreach(Sound s in _sounds)
        {
            s._source = gameObject.AddComponent<AudioSource>();
            s._source.volume = s._volume;
            s._source.spatialBlend = s._spatialBlend;
            s._source.pitch = s._pitch;
            s._source.clip = s._audioClip;
            s._source.loop = s._Shouldloop;
           
        }

        _footSteps = new string[] { "FootStep1", "FootStep2", "FootStep3", "FootStep4" };
    }
    #endregion

    #region Public Method
    public void PlaySFX(string name, float volume = 0)
    {
        Sound clip = GetClip(name);
        if (clip != null)
        {
            if (volume != 0) clip._source.volume = volume;
            clip._source.Play();
        }
    }

    public void StopPlayingAudio(string name)
    {
        Sound clip = GetClip(name);
        if (clip != null) clip._source.Stop();
    }

    Sound GetClip(string name)
    {
        Sound clip = Array.Find(_sounds, sound => sound._name == name);
        if (clip == null)
        {
            Debug.LogWarning("Sound of name " + name + " doesn't exist!");
            return null;
        }
        return clip;
    }

    public void FadeInAudio(string name)
    {
        Sound clip = GetClip(name);
        StartCoroutine(FadeInMusic(clip));
    }

    public void FadeOutAudio(string name)
    {
        Sound clip = GetClip(name);
        StartCoroutine(FadeOutMusic(clip));
    }

    IEnumerator FadeInMusic(Sound s)
    {
        s._source.volume = 0;
        while (s._source.volume < s._volume)
        {
            s._source.volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        s._source.volume = s._volume;
        s._source.Play();
    }

    IEnumerator FadeOutMusic(Sound s)
    {
        while (s._source.volume > 0)
        {
            s._source.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        s._source.Stop();
    }

    public void PlayMovementSound(float volume)
    {
        int n = UnityEngine.Random.Range(1, _footSteps.Length);
        string soundName = _footSteps[n];
        PlaySFX(soundName,volume);
        _footSteps[n] = _footSteps[0];
        _footSteps[0] = soundName;
    }

    #endregion


}
