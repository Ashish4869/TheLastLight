using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the gameobject after the animation has been played
/// </summary>

public class AutoDestroyAfterAnimation : MonoBehaviour
{
    #region MonoBehaviour CallBacks
    private void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
    #endregion
}
