using UnityEngine;

public class PickUpObjects : Interactable
{
    #region Variables
    public Weapon.Gun Gun;
    public Weapon.AmmoType ammo;
    public Notification _notif;
    #endregion

    #region Private Methods
    protected override void Interact()
    {
        base.Interact();

        Player player = FindAnyObjectByType<Player>();

        if (player != null)
        {
            if (Weapon.Gun.NotGun == Gun)
            {
                if (ammo == Weapon.AmmoType.Med)
                {
                    GameManager.Instance.PickedUpHealthPack();
                    AudioManager.Instance.PlaySFX("MedPickUp");
                    Destroy(gameObject);
                    return;
                }

                player.PickedUpWeaponAmmo(ammo,_notif);
            }
            else
            {
                player.PickedUpWeapon(Gun);
            }
        }
        Destroy(gameObject);
    }


    #endregion
}
