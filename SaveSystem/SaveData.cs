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

    //setters
    public void SetAKBool(bool condition) => _hasAK = condition;
    public void SetShotGunBool(bool condition) => _hasShotGun = condition;
    public void SetCurrentLevel(int level) => _currentLevel = level;
    public void SetPistolBullets(int PistolBullets) => _pistolBullets = PistolBullets;
    public void SetAKBullets(int AKBullets) => _AKBullets = AKBullets;
    public void SetShotGunBullets(int ShotGunBullets) => _shotGunBullets = ShotGunBullets;
    public void SetIsInCarBool(bool condition) => _isInCar = condition;


    //getters
    public bool GetAKBool() => _hasAK;
    public bool GetShotGunBool() => _hasShotGun;
    public int GetCurrentLevel() => _currentLevel;

    public int GetPistolBullets() => _pistolBullets;
    public int GetShotGunBullets() => _shotGunBullets;
    public int GetAKBullets() => _AKBullets;
    public bool GetIsInCarBool() => _isInCar;


    // private methods
    public  void SaveValuesInPC()
    {
        StartCoroutine(SaveValues());
    }

    IEnumerator SaveValues()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Data saved in disk");
        SaveSystem.SaveGameData(this);

    }

}
