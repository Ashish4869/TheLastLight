using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Toggles PostProcessing as per the game settings
/// </summary>
public class TogglePostProcessing : MonoBehaviour
{
    PostProcessLayer _ppv;
    // Start is called before the first frame update
    void Awake()
    {
        _ppv = GetComponent<PostProcessLayer>();
    }

    
    public void TogglePostProcessingVolume(bool condition)
    {
        _ppv.enabled = condition;
    }
}
