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
    bool _hasAK, _hasShotGun,_isInCar, _canPlayCutscene, _isBossLevel;
    int _currentLevel,_pistolBullets, _AKBullets, _shotGunBullets, _cutsceneIndex;
    bool[] _zombieStatus, _crateStatus, _objectiveStatus, _disposableStatus;
    float _playerPosX, _playerPosY, _playerPosZ;

    //setters
    public void SetAKBool(bool condition) => _hasAK = condition;
    public void SetShotGunBool(bool condition) => _hasShotGun = condition;
    public void SetIsBossLevel(bool condition) => _isBossLevel = condition;
    public void SetCanPlayCutscene(bool condition) => _canPlayCutscene = condition;  
    public void SetCurrentLevel(int level) => _currentLevel = level;
    public void SetPistolBullets(int PistolBullets) => _pistolBullets = PistolBullets;
    public void SetAKBullets(int AKBullets) => _AKBullets = AKBullets;
    public void SetShotGunBullets(int ShotGunBullets) => _shotGunBullets = ShotGunBullets;
    public void SetIsInCarBool(bool condition) => _isInCar = condition;
    public void SetCutsceneIndex(int index) => _cutsceneIndex = index;
    public void SetZombieStatus(bool[] zombieStatus) => _zombieStatus = zombieStatus;
    public void SetCrateStatus(bool[] crateStatus) => _crateStatus = crateStatus;
    public void SetObjectiveStatus(bool[] objectiveStatus) => _objectiveStatus = objectiveStatus;

    public void SetDisposableStatus(bool[] disposableStatus) => _disposableStatus = disposableStatus;

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
    public bool GetIsBossLevel() => _isBossLevel;

    public bool GetCanPlayCutscene() => _canPlayCutscene;

    public int GetCutsceneIndex() => _cutsceneIndex;
    public bool[] GetZombieStatus() => _zombieStatus;

    public bool[] GetCrateStatus() => _crateStatus;

    public bool[] GetObjectiveStatus() => _objectiveStatus;

    public bool[] GetDisposableStatus() => _disposableStatus;

    public Vector3 GetPlayerPosition()
    {
        Vector3 postion = new Vector3(_playerPosX, _playerPosY, _playerPosZ);
        return postion;
    }

    //Clearing data that should not persist between levels
    public void ClearNonLevelPersistantData()
    {
        _zombieStatus = null;
        _crateStatus = null;
        _objectiveStatus = null;
        _disposableStatus = null;
        _canPlayCutscene = true;
        _cutsceneIndex = 0;
        _playerPosX = 0;
        _playerPosY = 0;
        _playerPosZ = 0;
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

    private void OnDestroy()
    {
        EventManager.OnCheckPointReached -= SaveValuesInPC;
    }

}
