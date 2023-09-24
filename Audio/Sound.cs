using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
    public string _name;
    [Range(0,1)] public float _volume;
    [Range(0,1)] public float _spatialBlend;
    [Range(0,2)] public float _pitch;
    public AudioClip _audioClip;
    public bool _Shouldloop;
    [SerializeField] public AudioMixerGroup _mixer;

    [HideInInspector] public AudioSource _source;
}
