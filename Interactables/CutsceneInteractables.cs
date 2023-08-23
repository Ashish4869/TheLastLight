using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interacting an object that results in a cutscene
/// </summary>

public class CutsceneInteractables : Interactable
{
    protected override void Interact()
    {
        base.Interact();
        //play the cutscene
        UIManager.Instance.ClearButtonPrompt();
        GameManager.Instance.PlayCutscene();
        
    }
}
