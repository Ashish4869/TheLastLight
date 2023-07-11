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

    //Fired when the player dies
    public void OnPlayerDeathEvent()
    {
        if(OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    
}
