using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks how much zombies are remaining in the scene
/// </summary>

public class ZombieCounter : MonoBehaviour
{
    [SerializeField] Notification _notif;
    [SerializeField] GameObject _enemyBoss;
    private void Start()
    {
        InvokeRepeating("CheckIfAllZombieDead", 10, 5);
    }

    void CheckIfAllZombieDead()
    {
        if(transform.childCount == 0)
        {
            _enemyBoss.SetActive(true);
            UIManager.Instance.TriggerNotification(_notif);
            ObjectiveManager.Instance.UpdateObjectivePage(13, 0, true);
            GameManager.Instance.PlayCutscene();
            CancelInvoke();
        }
    }
}
