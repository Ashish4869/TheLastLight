using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispoableItemManager : ObjectStatusParent
{
    bool[] _disposableStatus;
    private void Start()
    {
        _disposableStatus = base.StartUp(transform, _disposableStatus, SetUpDisposableStatus);
    }

    public void UpdateDisposableStatus()
    {
        base.UpdateStatus(transform, SaveData.Instance.SetZombieStatus, _disposableStatus);
    }

    public bool[] SetUpDisposableStatus()
    {
        _disposableStatus = base.SetUpStatus(transform, SaveData.Instance.GetZombieStatus);
        return _disposableStatus;
    }
}
