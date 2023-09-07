using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which holds the data that needs to be saved between sessions
/// </summary>
[System.Serializable]
public class GameData
{
    public bool _hasAK, _hasShotGun, _isInCar;
    public int _currentLevel, _pistolBullets, _AKBullets, _shotGunBullets;
    public GameData(SaveData saveData)  
    {
        _hasAK = saveData.GetAKBool();
        _hasShotGun = saveData.GetShotGunBool();
        _currentLevel = saveData.GetCurrentLevel();
        _pistolBullets = saveData.GetPistolBullets();
        _shotGunBullets = saveData.GetShotGunBullets();
        _AKBullets = saveData.GetAKBullets();
        _isInCar = saveData.GetIsInCarBool();
    }
}
