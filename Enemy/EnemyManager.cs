using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a track of the zombies in the game
/// </summary>
public class EnemyManager : ObjectStatusParent
{
    bool[] _zombieStatus;
    private void Start()
    {
        _zombieStatus = base.StartUp(transform,_zombieStatus,SetUpZombiesStatus);
    }

    public void UpdateZombieStatus()
    {
        base.UpdateStatus(transform, SaveData.Instance.SetZombieStatus, _zombieStatus);
    }

    public bool[] SetUpZombiesStatus()
    {
        _zombieStatus = base.SetUpStatus(transform, SaveData.Instance.GetZombieStatus);
        return _zombieStatus;
    }


}
