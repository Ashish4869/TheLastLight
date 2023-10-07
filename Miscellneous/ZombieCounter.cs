using System;
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
    bool _isEnemiesDead;
    bool _isBossDead;


   
    private void Start()
    {
        _enemyBoss.SetActive(false);
        if (GameManager.Instance.HasValueFromDisk())
        {
            SetUpValuesFromDisk();
            if (SaveData.Instance.GetIsBossLevel())
            {
                ObjectiveManager.Instance.UpdateObjectivePage(13, 0, true);
                return;
            }
        }

        if(!_isEnemiesDead)
        {
            InvokeRepeating("CheckIfAllZombieDead", 10, 5);
        }
       
    }

    private void SetUpValuesFromDisk()
    {
        _isEnemiesDead = SaveData.Instance.GetIsLevel3ZombiesDead();
        _isBossDead = SaveData.Instance.GetIsLevel3BossDead();
    }

    void CheckIfAllZombieDead()
    {   
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) return; //if any of the children happens to be active in scene, return
        }

        _isEnemiesDead = true;
        SaveData.Instance.SetIsLevel3ZombiesDead(_isEnemiesDead);

        
        UIManager.Instance.TriggerNotification(_notif);
        ObjectiveManager.Instance.UpdateObjectivePage(13, 0, true);
        GameManager.Instance.PlayCutscene();
        SaveData.Instance.SetIsBossLevel(true);
        FindAnyObjectByType<EventManager>().OnCheckPointReachedEvent();
        CancelInvoke();
    }

    public void SpawnBoss() //is called when the second cutscene is over
    {
        _enemyBoss.SetActive(true);
    }

    public bool AreEnemiesDead() => _isEnemiesDead;
    public bool IsBossDead() => _isBossDead;
}
