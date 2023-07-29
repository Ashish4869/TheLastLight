using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DisablesCar if its totalled
/// </summary>
public class CarTotalledCheck : MonoBehaviour
{
    CarMovement _carMovement;
    // Start is called before the first frame update
    void Start()
    {
        _carMovement = GetComponentInParent<CarMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.CompareTag("Ground")) _carMovement.CarTotalled();

    }
}
