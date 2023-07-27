using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves the car based on input
/// Checks for input for exiting car and changing gears
/// Manages Engine Sounds to be played
/// </summary>

public class CarMovement : MonoBehaviour
{
    float _normalSpeed = 10, _turnForce = 100f, _speed = 0f, _highGearSpeed = 15f;
    bool _IsInHighGear = false;
    string _audioName, _prevAudioName = "Null";

    [SerializeField] Transform _frontLeftWheel;
    [SerializeField] Transform _frontRightWheel;
    [SerializeField] Transform _rearLeftWheel;
    [SerializeField] Transform _rearRightWheel;
    CarGearUIManager _carGearUIManager;
    BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = (BoxCollider)GetComponent("BoxCollider");
    }
    private void Start()
    {
        _carGearUIManager = (CarGearUIManager)GetComponent("CarGearUIManager");
    }

    private void OnEnable()
    {
        _boxCollider.enabled = true;
    }

    private void OnDisable()
    {
        _boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckForInput();
        PlayEngineSound();
    }

    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) //Get out of the car
        {
            GetOutOfCar();
            DisableCar();
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
}
