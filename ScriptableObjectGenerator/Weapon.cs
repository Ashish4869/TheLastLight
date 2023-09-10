using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon" , menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public enum Gun
    {
        Axe,
        Pistol,
        AK47,
        ShotGun,
        NotGun = -1
    };

    public enum AmmoType
    {
        Pistol = 1,
        AK47 = 2,
        Shotgun = 3,
        Med = 4
    };

    public string Name;
    public GameObject prefab;
    public int aimSpeed;
    public Gun gun;
    public AmmoType Ammo;
    public int pellets;
    public float Firerate;
    public float ADSFireRate;
    public float Spray;
    public float ADSSpray;
    public float RUNSpray;
    public float EquipTime;
    public float Recoil;
    public float KickBack;
    public int ammo;
    public int ClipSize;
    public float ReloadTime;
    public float SoundIntensity;
    public float Damage;

    [Range(0, 1)] public float MainCamFov;
    [Range(0, 1)] public float GunCamFov;

    public bool Recovery;

    private int CurrentTotalAmmo;
    private int ClipAmmo;
    private float CurrentSpray;
    private float CurrentFireRate;
    private bool Running;

    
    public void Initialise()
    {
        CurrentTotalAmmo = ammo;
        ClipAmmo = ClipSize;
        CurrentFireRate = Firerate;
    }
    public bool FireBullet()
    {
        if(ClipAmmo > 0)
        {
            ClipAmmo -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reload()
    {
        CurrentTotalAmmo += ClipAmmo;
        ClipAmmo = Mathf.Min(CurrentTotalAmmo, ClipSize);
        CurrentTotalAmmo -= ClipAmmo;
    }

    public bool StashEmpty()
    {
        if(CurrentTotalAmmo == 0)
        {
            return true;
        }
        return false;
    }

    public bool IsEmpty()
    {
        if(ClipAmmo == 0)
        {
            return true;
        }

        return false;
    }

    public bool IsFull()
    {
        if(ClipAmmo == ClipSize)
        {
            return true;
        }

        return false;
    }

    public void ADSShift(bool ADS)
    {
        if (ADS == true)
        {
            CurrentSpray = ADSSpray;
            CurrentFireRate = ADSFireRate;
        }
        else  if(Running == false)
        {
            CurrentSpray = Spray;
            CurrentFireRate = Firerate;
        }
    }

    public void RUNShift(bool Run)
    {
        if (Run == true)
        {
            CurrentSpray = RUNSpray;
            Running = true;
        }
        else
        {
            Running = false;
        }
    }

    public void UpdateAmmo(int Bullets)
    {
       if(Bullets + CurrentTotalAmmo > ammo)
       {
            CurrentTotalAmmo = ammo;
       }
       else
       {
            CurrentTotalAmmo += Bullets;
       }
    }


    //Getters
    public float GetCurrentAmmoBarValue() => (float)(CurrentTotalAmmo + ClipAmmo) / (ammo + ClipSize);
    public int GetTotalCurrentAmmo() => CurrentTotalAmmo + ClipAmmo;
    public int GetClipAmmo() => ClipAmmo;
    public float GetCurrentSpray() => CurrentSpray;
    public float GetCurrentFireRate() => CurrentFireRate;

    //setters
    public void SetTotalAmmo(int remainingAmmo)
    {
        CurrentTotalAmmo = (remainingAmmo - ClipSize);
        ClipAmmo = ClipSize;
    }
}
