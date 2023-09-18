using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which holds the data that needs to be saved between sessions
/// </summary>
[System.Serializable]
public class GameData
{
    //data to be saved
    public bool _hasAK, _hasShotGun, _isInCar, _canPlaycutscene, _isBossLevel;
    public int _currentLevel, _pistolBullets, _AKBullets, _shotGunBullets, _cutsceneIndex;
    public bool[] _zombieStatus, _crateStatus, _objectiveStatus, _disposableStatus;
    public float _playerPosX, _playerPosY, _playerPosZ;
    public GameData(SaveData saveData)  
    {
        _hasAK = saveData.GetAKBool();
        _hasShotGun = saveData.GetShotGunBool();
        _canPlaycutscene = saveData.GetCanPlayCutscene();
        _isBossLevel = saveData.GetIsBossLevel();
        _currentLevel = saveData.GetCurrentLevel();
        _pistolBullets = saveData.GetPistolBullets();
        _shotGunBullets = saveData.GetShotGunBullets();
        _AKBullets = saveData.GetAKBullets();
        _isInCar = saveData.GetIsInCarBool();
        _cutsceneIndex = saveData.GetCutsceneIndex();
        _zombieStatus = saveData.GetZombieStatus();
        _crateStatus = saveData.GetCrateStatus();
        _objectiveStatus = saveData.GetObjectiveStatus();
        _disposableStatus = saveData.GetDisposableStatus();
        _playerPosX = saveData.GetPlayerPosition().x;
        _playerPosY = saveData.GetPlayerPosition().y;
        _playerPosZ = saveData.GetPlayerPosition().z;
    }
}
