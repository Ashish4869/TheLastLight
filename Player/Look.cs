using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Handles Look and rotation
/// </summary>
public class Look : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Transform _playerEyes;
    [SerializeField]
    Transform _WeaponCam;
    Quaternion _originalCam;
    float _mouseSensitivity = 1.5f;
    Transform _weapon;
    #endregion

    #region MonoBehaviourCallback
    // Start is called before the first frame update
    void Start()
    {

        _originalCam = _playerEyes.rotation;
        _weapon = GameObject.Find("Weapon").GetComponent<Transform>();

        EventManager.OnPlayerDeath += Die;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GamePaused()) return;

        LookX();
        LookY();

        _WeaponCam.rotation = _playerEyes.rotation;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= Die;
    }
    #endregion

    #region Private Functions

    void Die()
    {
        enabled = false;
    }
    void LookX()
    {
        float LookX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        Quaternion Xangle = Quaternion.AngleAxis(LookX, Vector3.up);
        Quaternion XangleTemp = Xangle * transform.localRotation;
        transform.localRotation = XangleTemp;
    }

    void LookY()
    {
        float MaxAngle = 60f;
        float LookY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
        Quaternion Yangle = Quaternion.AngleAxis(LookY, -Vector3.right);
        Quaternion YangleTemp = Yangle * _playerEyes.localRotation;

        if(Quaternion.Angle(_originalCam , YangleTemp) < MaxAngle)
        {
            _playerEyes.localRotation = YangleTemp;
        }
        _weapon.rotation = _playerEyes.rotation;
        
    }
    #endregion
}
