using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kills an enemy that collides with this gameobject
/// </summary>
public class KillEnemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hello");
            collision.gameObject.GetComponent<RagdollDeath>().EnableRagDollEffect(Vector3.up,collision.rigidbody);
        }
    }
}
