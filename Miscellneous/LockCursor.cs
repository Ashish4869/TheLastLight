using UnityEngine;
using UnityEngine.SceneManagement;

public class LockCursor : MonoBehaviour
{
    #region Variables
    bool _cursorLock = true;
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            EnableCursor();
            return;
        }

        UpdateCursorLock();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 4) return;

        if (GameManager.Instance.DialougeStatus()) return;

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

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _cursorLock = true;
    }

    #endregion
}
