using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Sets up the notification data
/// </summary>

public class SetupNotification : MonoBehaviour
{
    #region Variables
    [SerializeField] Image _NotificationImage;
    [SerializeField] TextMeshProUGUI _NotificationText;
    #endregion

    #region MonoBehaviourCallBacks
  
    #endregion

    #region Public Methods
    public void SetUpNotification(Notification notification, string NotificationMessage)
    {
        //setting up image
        _NotificationImage.sprite = notification.GetNotificationSprite();

        //seting up text
        if(NotificationMessage == "")
        {
            _NotificationText.text = notification.GetNotificationString();
        }
        else
        {
            _NotificationText.text = NotificationMessage;
        }
    }
    #endregion
}
