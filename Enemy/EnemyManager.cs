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
        InitializeZombieArray();
        if (GameManager.Instance.HasValueFromDisk()) SetUpZombiesStatus();
    }

    private void InitializeZombieArray()
    {
        int zombieCount = transform.childCount;
        _zombieStatus = new bool[zombieCount];

        for (int i = 0; i < zombieCount; i++)
        {
            _zombieStatus[i] = true;
        }
    }

    public void UpdateZombieStatus()
    {
        int zombieCount = transform.childCount;

        for (int i = 0; i < zombieCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _zombieStatus[i] = false;
            }
        }

        SaveData.Instance.SetZombieStatus(_zombieStatus);   
    }

    public void SetUpZombiesStatus()
    {
        if (SaveData.Instance.GetZombieStatus() == null) return;

        _zombieStatus = SaveData.Instance.GetZombieStatus();

        for (int i = 0; i < _zombieStatus.Length; i++)
        {
            if (_zombieStatus[i] == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


}
