using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps Track of the crates in the level
/// </summary>
public class CrateManager : MonoBehaviour
{
    bool[] _crateStatus;
    private void Start()
    {
        InitializeCrateArray();
        if (GameManager.Instance.HasValueFromDisk()) SetUpCrateStatus();
    }

    private void InitializeCrateArray()
    {
        int crateCount = transform.childCount;
        _crateStatus = new bool[crateCount];

        for (int i = 0; i < crateCount; i++)
        {
            _crateStatus[i] = true;
        }
    }

    public void UpdateCrateStatus()
    {
        int crateCount = transform.childCount;

        for (int i = 0; i < crateCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _crateStatus[i] = false;
            }
        }

        SaveData.Instance.SetCrateStatus(_crateStatus);
    }

    public void SetUpCrateStatus()
    {
        if (SaveData.Instance.GetCrateStatus() == null) return;

        _crateStatus = SaveData.Instance.GetCrateStatus();

        for (int i = 0; i < _crateStatus.Length; i++)
        {
            if (_crateStatus[i] == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
