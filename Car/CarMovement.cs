using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves the car based on input
/// Checks for input for exiting car and changing gears
/// Manages Engine Sounds to be played
/// Size of the collider which attracts zombies 
/// </summary>

public class CarMovement : MonoBehaviour
{
    #region Variables
    float _normalSpeed = 10, _turnForce = 100f, _speed = 0f, _highGearSpeed = 15f;
    bool _IsInHighGear = false, _carTotalled = false, _canDrive = true;
    string _audioName, _prevAudioName = "Null";

    [Header("Wheels")]
    [SerializeField] Transform _frontLeftWheel;
    [SerializeField] Transform _frontRightWheel;
    [SerializeField] Transform _rearLeftWheel;
    [SerializeField] Transform _rearRightWheel;

    [Header("Lights")]
    [SerializeField] Light _frontLeftLight;
    [SerializeField] Light _frontRightLight;
    [SerializeField] Light _rearLeftLight;
    [SerializeField] Light _rearRightLight;

    [Header("Notification")]
    [SerializeField] Notification _notif;


    CarGearUIManager _carGearUIManager;
    BoxCollider _boxCollider;
    EventManager _eventManager;
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        _boxCollider = (BoxCollider)GetComponent("BoxCollider");
        _eventManager = FindAnyObjectByType<EventManager>();

        EventManager.OnStartCutscene += DisableCarbeforeCutscene;
        EventManager.OnEndCutscene += EnableCarAfterCutscene;
    }
    private void Start()
    {
        _carGearUIManager = (CarGearUIManager)GetComponent("CarGearUIManager");
    }

    private void OnEnable()
    {
        _boxCollider.enabled = true;
        _frontLeftLight.enabled = true;
        _frontRightLight.enabled = true;
        _rearLeftLight.enabled = true;
        _rearRightLight.enabled = true;

        gameObject.tag = "Player";
        _eventManager.OnPlayerEnterExitCarEvent();
    }

    private void OnDisable()
    {
        _boxCollider.enabled = false;
        _frontLeftLight.enabled = false;
        _frontRightLight.enabled = false;
        _rearLeftLight.enabled = false;
        _rearRightLight.enabled = false;

        gameObject.tag = "Untagged";
        _eventManager.OnPlayerEnterExitCarEvent();
    }

    private void OnDestroy()
    {
        EventManager.OnStartCutscene -= DisableCarbeforeCutscene;
        EventManager.OnEndCutscene -= EnableCarAfterCutscene;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canDrive) return;

        Movement();
        CheckForInput();
        PlayEngineSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (other.gameObject != null)
            {
                other.gameObject.GetComponentInParent<EnemyAI>().SensedPlayer();
            }
        }
    }
    #endregion

    #region Private Methods
    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.E) || _carTotalled) //Get out of the car
        {
            GetOutOfCar();
            DisableCar();

            if (_carTotalled) UIManager.Instance.TriggerNotification(_notif);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _IsInHighGear = !_IsInHighGear;
            AudioManager.Instance.PlaySFX("ShiftGear");
            _carGearUIManager.UpdateGearUI(_IsInHighGear);
        }
    }

    private void DisableCar()
    {
        
        Transform parent = transform.parent;
        Transform CameraPivot = parent.Find("CameraPivot");
        ResetCarInteractablePostion(parent);
        DisableCamera(CameraPivot);
        AudioManager.Instance.StopPlayingAudio(_audioName); 
        enabled = false;
    }

    private void ResetCarInteractablePostion(Transform parent)
    {
        //reseting the parent to where the car model has gone
        parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }

    private void DisableCamera(Transform CameraPivot)
    {
        CameraPivot.localPosition = Vector3.zero;
       CameraPivot.gameObject.SetActive(false);
    }

    private void GetOutOfCar()
    {
        GameManager.Instance.TransitionToPlayer();
        GameManager.Instance.PlacePlayerNearCar(transform.position);
        UIManager.Instance.EnableUIFromCar();
    }

    private void Movement()
    {
        if (_carTotalled) return;

        //Set speed as per gear
        _speed = _IsInHighGear ? _highGearSpeed : _normalSpeed;
        _audioName = _IsInHighGear ? "CarForwardHigh" : "CarForward";

        //Input
        float HInput = Input.GetAxis("Horizontal");
        float Vinput = Input.GetAxis("Vertical");

        MoveWheels(HInput, Vinput);

        //Application
        if (Vinput == 0) return;
        transform.Translate(Vector3.forward * Vinput * Time.deltaTime * _speed);
        transform.Rotate(Vector3.up * HInput * Time.deltaTime * _turnForce);
    }

    private void MoveWheels(float hInput, float vinput)
    {
        if (vinput == 0)
        {
            _audioName = "CarIdle";
            return; //no need to move wheels if the car is not moving
        }

        bool IsCarMovingForward = vinput > 0 ? true : false;
        float TurnRotationAmountY = 30f;

        TurnWheels(IsCarMovingForward);

        if (hInput == 0)
        {
            _frontLeftWheel.localRotation = Quaternion.Slerp(_frontLeftWheel.localRotation, Quaternion.Euler(0, 0, 0), _speed * Time.deltaTime);
            _frontRightWheel.localRotation = Quaternion.Slerp(_frontRightWheel.localRotation, Quaternion.Euler(0, 0, 0), _speed * Time.deltaTime);
            return;
        } 

        else TurnRotationAmountY = hInput > 0 ? TurnRotationAmountY : -TurnRotationAmountY;

        if (vinput < 0) TurnRotationAmountY = -TurnRotationAmountY;

        _frontLeftWheel.localRotation = Quaternion.Slerp(_frontLeftWheel.localRotation, Quaternion.Euler(0, TurnRotationAmountY, 0), _speed * Time.deltaTime);
        _frontRightWheel.localRotation = Quaternion.Slerp(_frontRightWheel.localRotation, Quaternion.Euler(0, TurnRotationAmountY, 0), _speed * Time.deltaTime);
        
    }

    private void PlayEngineSound()
    {
        if (_prevAudioName != _audioName)
        {
            AudioManager.Instance.StopPlayingAudio(_prevAudioName);
            AudioManager.Instance.PlaySFX(_audioName);
            _prevAudioName = _audioName;
        }
       
    }

    private void TurnWheels(bool isCarMovingForward)
    {
        float speed = isCarMovingForward ? _speed : -_speed;
        Quaternion rotationAmount = Quaternion.AngleAxis(speed, Vector3.right);
        _frontLeftWheel.localRotation *= rotationAmount;
        _frontRightWheel.localRotation *= rotationAmount;
        _rearLeftWheel.localRotation *= rotationAmount;
        _rearRightWheel.localRotation *= rotationAmount;
    }


    void DisableCarbeforeCutscene()
    {
        _canDrive = false;
    }

    void EnableCarAfterCutscene()
    {
        _canDrive = true;
    }
    #endregion

    #region Public Methods
    public void CarTotalled() => _carTotalled = true;
    #endregion
}
