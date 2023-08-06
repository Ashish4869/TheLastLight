using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Circles Around the map and lands when all enemies are dead.
/// </summary>
public class Helicopter : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
