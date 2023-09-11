using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a track of the zombies in the game
/// </summary>
public class EnemyManager : MonoBehaviour
{
    bool[] _zombieStatus;
    private void Start()
    {
        int zombieCount = transform.childCount - 1;
        _zombieStatus = new bool[zombieCount];

        for(int i = 0; i < zombieCount; i++)
        {
            _zombieStatus[i] = true;
        }


        if (GameManager.Instance.HasValueFromDisk()) SetUpZombiesStatus();
    }

    public void UpdateZombieStatus()
    {
        int zombieCount = transform.childCount - 1;

        for (int i = 0; i < zombieCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _zombieStatus[i] = false;
                Debug.Log("Zombie No:" + i + "Is dead and saved in to saveData");
            }
        }

        SaveData.Instance.SetZombieStatus(_zombieStatus);
    }

    public void SetUpZombiesStatus()
    {
        _zombieStatus = SaveData.Instance.GetZombieStatus();

        if (_zombieStatus == null) return;

        for(int i = 0; i < _zombieStatus.Length; i++)
        {
            if(_zombieStatus[i] == false)
            {
                Debug.Log("Zombie No: " + i + "Is Deactivated");
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }


}
