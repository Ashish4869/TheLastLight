using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the events to be taken care of after the boss is dead
/// udpate objective book
/// move the helicopter to landing position
/// </summary>

public class BossHandler : MonoBehaviour
{
    [SerializeField] Notification _notif;
    EventManager _eventManager;

    private void Awake()
    {
        _eventManager = FindAnyObjectByType<EventManager>();
    }
    

  

    public void HandlePostBossDeath()
    {
        _eventManager.OnBossDefeatedEvent();
        UIManager.Instance.TriggerNotification(_notif);
        ObjectiveManager.Instance.UpdateObjectivePage(14, 0, true);
    }
}
