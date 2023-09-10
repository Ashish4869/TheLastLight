using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //Implementation of Singleton Pattern
    private static SaveData _instance;
    public static SaveData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveData>();

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<SaveData>();
                }
            }

            return _instance;
        }

    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        EventManager.OnCheckPointReached += SaveValuesInPC;
    }

   

    //Data to save when the player tries to save the game.
    bool _hasAK, _hasShotGun,_isInCar;
    int _currentLevel,_pistolBullets, _AKBullets, _shotGunBullets;
    bool[] _zombieStatus, _crateStatus;
    float _playerPosX, _playerPosY, _playerPosZ;

    //setters
    public void SetAKBool(bool condition) => _hasAK = condition;
    public void SetShotGunBool(bool condition) => _hasShotGun = condition;
    public void SetCurrentLevel(int level) => _currentLevel = level;
    public void SetPistolBullets(int PistolBullets) => _pistolBullets = PistolBullets;
    public void SetAKBullets(int AKBullets) => _AKBullets = AKBullets;
    public void SetShotGunBullets(int ShotGunBullets) => _shotGunBullets = ShotGunBullets;
    public void SetIsInCarBool(bool condition) => _isInCar = condition;
    public void SetZombieStatus(bool[] zombieStatus) => _zombieStatus = zombieStatus;
    public void SetCrateStatus(bool[] crateStatus) => _crateStatus = crateStatus;

    public void SetPlayerPosition(Vector3 position)
    {
        _playerPosX = position.x;
        _playerPosY = position.y;
        _playerPosZ = position.z;
    }
   


    //getters
    public bool GetAKBool() => _hasAK;
    public bool GetShotGunBool() => _hasShotGun;
    public int GetCurrentLevel() => _currentLevel;

    public int GetPistolBullets() => _pistolBullets;
    public int GetShotGunBullets() => _shotGunBullets;
    public int GetAKBullets() => _AKBullets;
    public bool GetIsInCarBool() => _isInCar;
    public bool[] GetZombieStatus() => _zombieStatus;

    public bool[] GetCrateStatus() => _crateStatus;

    public Vector3 GetPlayerPosition()
    {
        Vector3 postion = new Vector3(_playerPosX, _playerPosY, _playerPosZ);
        return postion;
    }


    // private methods
    public  void SaveValuesInPC()
    {
        StartCoroutine(SaveValues());
    }

    IEnumerator SaveValues()
    {
        yield return new WaitForSeconds(1f);
        SaveSystem.SaveGameData(this);
    }

}
