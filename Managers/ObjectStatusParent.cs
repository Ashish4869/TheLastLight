using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class that handles the setting up of data
/// </summary>
public class ObjectStatusParent : MonoBehaviour
{
    public delegate bool[] GetData();
    public delegate bool[] SetUpData();

    public bool[] StartUp(Transform transform, bool[] statusbool, SetUpData setUpDataMethod)
    {
        if (GameManager.Instance.HasValueFromDisk()) statusbool = setUpDataMethod();
        Debug.Log("Got values from disk");

        if(statusbool == null)
        {
            int count = transform.childCount - 1;
            statusbool = new bool[count];

            for (int i = 0; i < count; i++)
            {
                statusbool[i] = true;
            }

            Debug.Log("returning initial values");
            return statusbool;
        }

        return null;
    }
    public void UpdateStatus(Transform transform, Action<bool[]> saveIntoSaveData, bool[] _statusBool)
    {
        int count = transform.childCount - 1;

        for (int i = 0; i < count; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                _statusBool[i] = false;
            }
        }

        saveIntoSaveData(_statusBool);
    }

    public bool[] SetUpStatus(Transform transform, GetData getData)
    {
        bool[] _statusBool = getData();

        if (_statusBool == null) return null;

        for(int i = 0; i < _statusBool.Length; i++)
        {
            if(_statusBool[i] == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        return _statusBool;

    }
}
