using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [SerializeField] Transform _car;
    float _movementSmoothness = 5f;
    float _turnSmoothness = 5f;
   

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _car.transform.position, _movementSmoothness * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _car.rotation, _turnSmoothness * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
