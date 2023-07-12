using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the sound that the zombie produces
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class EnemySoundManager : MonoBehaviour
{
    #region Variables
    AudioSource _source;
    EnemyState _enemyState = EnemyState.Wandering;
    [SerializeField] AudioClip _idle;
    [SerializeField] AudioClip _chase;
    [SerializeField] AudioClip _die;
    #endregion


    #region MonoBehaviour CallBacks
    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _idle;
        _source.Play();
    }
    #endregion


    #region Public Variables
    public void StateChanged(EnemyState state)  
    {
        if (_enemyState == state) return;

        _enemyState = state;

        switch(state)
        {
            case EnemyState.Wandering:
                IdleZombieSound();
                break;

            case EnemyState.Chasing:
                PlayerSpottedZombieSound();
                break;
        }
    }
    public void IdleZombieSound()
    {
        _source.clip = _idle;
        _source.Play();
    }
    public void PlayerSpottedZombieSound()
    {
        _source.clip = _chase;
        _source.Play();
    }

    public void DeathSound()
    {
        _source.clip = _die;
        _source.loop = false;
        _source.Play();
    }
    #endregion
}
    