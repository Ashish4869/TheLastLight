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
        int crateCount = transform.childCount - 1;
        _crateStatus = new bool[crateCount];

        for (int i = 0; i < crateCount; i++)
        {
            _crateStatus[i] = true;
        }


        if (GameManager.Instance.HasValueFromDisk()) SetUpCrateStatus();
    }

    public void UpdateCrateStatus()
    {
        int crateCount = transform.childCount - 1;

        for (int i = 0; i < crateCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _crateStatus[i] = false;
                Debug.Log("Create No: " + i + " saved in data");
            }
        }

        SaveData.Instance.SetCrateStatus(_crateStatus);
    }

    public void SetUpCrateStatus()
    {
        _crateStatus = SaveData.Instance.GetCrateStatus();

        if (_crateStatus == null) return;
            
        for (int i = 0; i < _crateStatus.Length; i++)
        {
            if (_crateStatus[i] == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
}
