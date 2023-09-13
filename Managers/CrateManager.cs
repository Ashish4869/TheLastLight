using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps Track of the crates in the level
/// </summary>
public class CrateManager : ObjectStatusParent
{
    bool[] _crateStatus;
    private void Start()
    {
        _crateStatus = base.StartUp(transform, _crateStatus, SetUpCrateStatus);
    }

    public void UpdateCrateStatus()
    {
        base.UpdateStatus(transform, SaveData.Instance.SetCrateStatus, _crateStatus);
    }

    public bool[] SetUpCrateStatus()
    {
        _crateStatus = base.SetUpStatus(transform, SaveData.Instance.GetCrateStatus);
        return _crateStatus;
    }
}
