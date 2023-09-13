using UnityEngine;

public class ObjectiveInteractables : Interactable
{
    public ObjectiveManager.ObjectiveCompletion _objectiveToComplete;
    public Notification _CompletionNotif, _inCompleteNotif;
    public ObjectiveManager.ObjectiveCompletionPrerequisite _objectivePrequisite;
    public bool _shouldShowInCompleteNotif;
    protected override void Interact()
    {
        base.Interact();
        RunObjective();
    }

    public  void RunObjective()
    {
        if(ObjectiveManager.Instance.CheckConditionForObjectiveCompletion(_objectiveToComplete, _CompletionNotif, _objectivePrequisite))
        {
            FindAnyObjectByType<DispoableItemManager>().UpdateDisposableStatus();
            gameObject.SetActive(false) ;
        }
        else
        {
           if(_shouldShowInCompleteNotif) UIManager.Instance.TriggerNotification(_inCompleteNotif);
        }
    }
   
}
