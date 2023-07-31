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

    
}
