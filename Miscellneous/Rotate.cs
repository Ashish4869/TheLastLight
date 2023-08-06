using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float _speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * _speed *  Time.deltaTime);
    }
}
