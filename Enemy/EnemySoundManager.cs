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
    EnemyState _enemyState = EnemyState.Searching;
    [SerializeField] AudioClip _idle;
    [SerializeField] AudioClip _chase;
    [SerializeField] AudioClip _die;
    int _waitbetweenSounds = 2;
    #endregion


    #region MonoBehaviour CallBacks
    void Awake()
    {
        _source = (AudioSource)GetComponent("AudioSource");
        _source.clip = _idle;
        _source.loop = false;
        _source.Play();
        StateChanged(EnemyState.Wandering);
    }
    #endregion


    #region Public Variables
    public void StateChanged(EnemyState state)  
    {
        if (_enemyState == state) return;

        _enemyState = state;
        StopAllCoroutines();
        switch(_enemyState)
        {
           
            case EnemyState.Wandering:
                StartCoroutine(IdleZombieSound());
                break;

            case EnemyState.Chasing:
                StartCoroutine(PlayerSpottedZombieSound());
                break;
        }
    }
    IEnumerator IdleZombieSound()
    {
        while(true)
        {
            _source.clip = _idle;
            _source.Play();
            _waitbetweenSounds = Random.Range(7, 10);
            yield return new WaitForSeconds(_waitbetweenSounds);
        }
        
    }
    IEnumerator PlayerSpottedZombieSound()
    {
        while(true)
        {
            _source.clip = _chase;
            _source.Play();
            _waitbetweenSounds = Random.Range(4, 7);
            yield return new WaitForSeconds(_waitbetweenSounds);
        }
       
    }

    public void DeathSound()
    {
        StopAllCoroutines();
        _source.clip = _die;
        _source.loop = false;
        _source.Play();
    }
    #endregion
}
    