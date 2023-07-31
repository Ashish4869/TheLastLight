using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the code for flashlight
/// </summary>
public class FlashLightHandler : MonoBehaviour
{
    Light _flashLight;
    bool _flashLightActivityStatus = false;

    private void Start()
    {
        _flashLight = (Light)GetComponent("Light");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _flashLightActivityStatus = !_flashLightActivityStatus;
            _flashLight.enabled = _flashLightActivityStatus;
        }
    }
}
