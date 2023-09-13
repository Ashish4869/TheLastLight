using UnityEngine;

public class PickUpObjects : Interactable
{
    #region Variables
    public Weapon.Gun Gun;
    public Weapon.AmmoType ammo;
    public Notification _notif;

    public bool _isObjectiveItem;
    public bool _isDisposableItem;
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
                    UIManager.Instance.TriggerNotification(_notif);
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

        if(_isObjectiveItem)
        {
            GetComponent<ObjectiveInteractables>().RunObjective();
        }

        if(_isDisposableItem)
        {
            FindAnyObjectByType<DispoableItemManager>().UpdateDisposableStatus();
            gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }


    #endregion
}
