using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello");
        if(other.CompareTag("GameObjective"))
        {
            FindAnyObjectByType<LevelLoader>().LoadNextLevel();
        }
    }
}
