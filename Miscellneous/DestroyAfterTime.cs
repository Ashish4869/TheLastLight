using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float _TimeAfterwhichToDestroy = 4f;
    
    void Start()
    {
        Destroy(gameObject, _TimeAfterwhichToDestroy);
    }

   
}
