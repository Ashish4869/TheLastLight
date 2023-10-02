using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows player to use the car
/// </summary>

public class Car : Interactable
{
    [SerializeField] GameObject _cameraPivot;
    protected override void Interact()
    {
        base.Interact();
        TransitionToCar();
        HidePlayerAndUI();
        AudioManager.Instance.PlaySFX("carDoorOpenClose");
        AudioManager.Instance.PlaySFX("StartCar");
    }

    private void HidePlayerAndUI()
    {
        GameManager.Instance.HidePlayer();
        UIManager.Instance.SetUpUIForCar();
    }

    void TransitionToCar()
    {
        CarMovement _carMovement = GetComponentInChildren<CarMovement>();
        _carMovement.enabled = true;
        _cameraPivot.SetActive(true);
    }

}
