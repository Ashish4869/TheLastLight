using UnityEngine;

public class ObjectiveInteractables : Interactable
{
    public ObjectiveManager.ObjectiveCompletion _objectiveToComplete;
    public Notification _notif;
    public ObjectiveManager.ObjectiveCompletionPrerequisite _objectivePrequisite;
    protected override void Interact()
    {
        base.Interact();
        RunObjective();
    }

    public  void RunObjective()
    {
        if(ObjectiveManager.Instance.CheckConditionForObjectiveCompletion(_objectiveToComplete, _notif, _objectivePrequisite))
        {
            Destroy(gameObject);
        }
        switch (_objectiveCompletion)
        {
            case ObjectiveManager.ObjectiveCompletion.ReceiveTaskFromOldMan:
                ObjectiveManager.Instance.OnCompleteAssignedOldManTask(_notif);
                break;

            case ObjectiveManager.ObjectiveCompletion.ReceiveTaskFromSuperMarketOwner:
                ObjectiveManager.Instance.OnCompleteAssignSuperMarketOwnerTask(_notif);
                break;

            case ObjectiveManager.ObjectiveCompletion.ObtainOldManCarKeys:
                if (ObjectiveManager.Instance.HasOldManMeds())
                {
                    ObjectiveManager.Instance.OnCompleteObtainOldManMeds(_notif);
                }
                break;

            case ObjectiveManager.ObjectiveCompletion.ObtainSuppliesFromSuperMarket:
                ObjectiveManager.Instance.OnCompleteObtainSuppliesFromSuperMarket(_notif);
                break;

            case ObjectiveManager.ObjectiveCompletion.ObtainManagerRoomKeys:
                if (ObjectiveManager.Instance.HasSupplies())
                {
                    ObjectiveManager.Instance.OnCompleteObtainedManagerRoomKeys(_notif);
                }
                break;

            case ObjectiveManager.ObjectiveCompletion.ObtainMedsForOldMan:
                ObjectiveManager.Instance.OnCompleteObtainedOldManMeds(_notif);
                break;

            case ObjectiveManager.ObjectiveCompletion.LeaveWellington:
                if(ObjectiveManager.Instance.HasCarKeys())
                {
                    UIManager.Instance.HideUI();
                    GameManager.Instance.PlayCutscene();
                }
                else
                {
                    UIManager.Instance.TriggerNotification(_notif);
                }
                break;
        }
    }
   
}
