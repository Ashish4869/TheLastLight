using UnityEngine;

public abstract class Interactable : MonoBehaviour
{   
    public string _promptMessage;
    
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact() { }
    
}
