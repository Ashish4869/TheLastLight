using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Doors : Interactable
{
    Animator _door;
    bool _isDoorOpen = false;
    [SerializeField] bool _isLocked;
    public Notification _notif;

    protected override void Interact()
    {
        base.Interact();
        if (_isLocked)
        {
            if (!ObjectiveManager.Instance.HasManagerRoomKeys())
            {
                UIManager.Instance.TriggerNotification(_notif);
                return;
            }
        }

        _door = GetComponent<Animator>();
        _isDoorOpen = !_isDoorOpen;
        _door.SetBool("IsOpen", _isDoorOpen);
    }

}
