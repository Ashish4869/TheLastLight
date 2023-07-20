using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Moves the car based on input

public class CarMovement : MonoBehaviour
{
    float _speed = 10, _turnForce = 100f;
    Rigidbody _rigibody;
    // Start is called before the first frame update
    void Start()
    {
        _rigibody = (Rigidbody)GetComponent("Rigidbody");
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        float HInput = Input.GetAxis("Horizontal");
        float Vinput = Input.GetAxis("Vertical");


        //Application
        transform.Translate(Vector3.forward * Vinput * Time.deltaTime * _speed);
        transform.Rotate(Vector3.up * HInput * Time.deltaTime * _turnForce);
    }
}
