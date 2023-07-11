using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunShots : MonoBehaviour
{
   public void PlayBulletAddSound()
    {
        AudioManager.Instance.PlaySFX("ShotGunBulletAdd");
    }

    public void PlayCockSound()
    {
        AudioManager.Instance.PlaySFX("ShotGunPump");
    }
}
