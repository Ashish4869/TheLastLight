using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deals with events triggered in the game
/// </summary>

public class EventManager : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void PlayerEnterExitCar();
    public static event PlayerEnterExitCar OnPlayerEnterExitCar;

    public delegate void BossDead();
    public static event BossDead OnBossDefeated;

    public delegate void StartCutscene();
    public static event StartCutscene OnStartCutscene;

    public delegate void EndCutscene();
    public static event EndCutscene OnEndCutscene;

    public delegate void CheckPointReached();
    public static event CheckPointReached OnCheckPointReached; 

    //Fired when the player dies
    public void OnPlayerDeathEvent()
    {
        if(OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    //Fired whenever the player enters and exits car
    public void OnPlayerEnterExitCarEvent()
    {
        if(OnPlayerEnterExitCar != null)
        {
            OnPlayerEnterExitCar();
        }
    }

    //Fired when we have killed the boss
    public void OnBossDefeatedEvent()
    {
        if(OnBossDefeated != null)
        {
            OnBossDefeated();
        }
    }

    //Fired when we start a cutscene
    public void OnStartCutsceneEvent()
    {
        if(OnStartCutscene != null)
        {
            OnStartCutscene();
        }
    }

    //Fired when a cutscene is over
    public void OnEndCutsceneEvent()
    {
        if(OnEndCutscene != null)
        {
            OnEndCutscene();
        }
    }

    //Fired when we reach a checkpoint
    public void OnCheckPointReachedEvent()
    {
        if(OnCheckPointReached != null)
        {
            OnCheckPointReached();
        }
    }
}
