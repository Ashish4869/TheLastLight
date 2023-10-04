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
        if(GameManager.Instance.HasValueFromDisk())
        {
            if(SaveData.Instance.GetIsBossLevel())
            {
                _enemyBoss.SetActive(true);
                ObjectiveManager.Instance.UpdateObjectivePage(13, 0, true);
                return;
            }
        }
        InvokeRepeating("CheckIfAllZombieDead", 10, 5);
    }

    void CheckIfAllZombieDead()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) return; //if any of the children happens to be active in scene, return
        }

        _isEnemiesDead = true;

        StartCoroutine(SpawnBoss());
        UIManager.Instance.TriggerNotification(_notif);
        ObjectiveManager.Instance.UpdateObjectivePage(13, 0, true);
        GameManager.Instance.PlayCutscene();
        SaveData.Instance.SetIsBossLevel(true);
        FindAnyObjectByType<EventManager>().OnCheckPointReachedEvent();
        CancelInvoke();
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(60); //wait for cutscene to end
        _enemyBoss.SetActive(true);
    }

    public bool AreEnemiesDead() => _isEnemiesDead;
    public bool IsBossDead() => _isBossDead;
}
