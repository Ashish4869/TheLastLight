using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispoableItemManager : MonoBehaviour
{
    bool[] _disposableStatus;
    private void Start()
    {
        if (GameManager.Instance.HasValueFromDisk()) SetUpDisposableStatus();
        InitializeDisposableArray();
    }

    private void InitializeDisposableArray()
    {
        int disposableCount = transform.childCount;
        _disposableStatus = new bool[disposableCount];

        for (int i = 0; i < disposableCount; i++)
        {
            _disposableStatus[i] = true;
        }
    }

    public void UpdateDisposableStatus()
    {
        int disposableCount = transform.childCount;

        for (int i = 0; i < disposableCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _disposableStatus[i] = false;
            }
        }

        SaveData.Instance.SetDisposableStatus(_disposableStatus);
    }

    public void SetUpDisposableStatus()
    {
        if (SaveData.Instance.GetDisposableStatus() == null) return;
        _disposableStatus = SaveData.Instance.GetDisposableStatus();

        for (int i = 0; i < _disposableStatus.Length; i++)
        {
            if (_disposableStatus[i] == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
