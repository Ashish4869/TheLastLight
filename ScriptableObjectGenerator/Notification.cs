using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New Notification Type", menuName = "Notification")]
public class Notification : ScriptableObject
{
    #region Variables
    [SerializeField] NotificationID _notifID;
    [SerializeField] Sprite _notifSprite;
    [SerializeField] string _NotificationMessage;
    #endregion

    #region Public Methods
    public NotificationID GetNotifID() => _notifID;
    public Sprite GetNotificationSprite() => _notifSprite;
    public string GetNotificationString() => _NotificationMessage;
    public void SetNotificationString(string message) => _NotificationMessage = message; 
    #endregion


}
