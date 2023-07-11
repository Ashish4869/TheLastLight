using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{
    #region Variables
    bool _cursorLock = true;
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        UpdateCursorLock();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) UpdateCursorLock();
    }
    #endregion


    #region Public Methods
    public void UpdateCursorLock()
    {
        if(_cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorLock = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorLock = true;
        }
    }

    #endregion
}
