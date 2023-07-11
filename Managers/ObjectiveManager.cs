using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    #region Singleton Implementation
    public static ObjectiveManager _instance;

    public static ObjectiveManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ObjectiveManager>();

                if (_instance == null) _instance = new GameObject().AddComponent<ObjectiveManager>();
            }

            return _instance;
        }
    }
    #endregion

    public enum ObjectiveCompletion
    {
        //Level 1
        ReceiveTaskFromOldMan,
        ObtainMedsForOldMan,
        ObtainOldManCarKeys,
        ReceiveTaskFromSuperMarketOwner,
        ObtainSuppliesFromSuperMarket,
        ObtainManagerRoomKeys,
        LeaveWellington
    }

    public enum ObjectiveCompletionPrerequisite
    {
        //Level 1
        None,
        ShouldHaveOldManMeds,
        ShouldHaveSupplies,
        ShouldHaveCarKeys,
        ShouldHaveManagerRoomKeys
    }

    #region Variables
    bool _hasOldManMeds = false;
    bool _hasSupplies = false;
    bool _hasCarKeys = false;
    bool _hasManagerRoomKeys = false;

    ObjectivePage _ObjectivePage;
    ObjectiveData _objectiveData = new ObjectiveData();
    #endregion

    #region MonoBehaviour CallBacks
    private void Awake()
    {
        _ObjectivePage = FindAnyObjectByType<ObjectivePage>();
    }

    private void Start()
    {
        _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[0], 0);
        _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[4], 0);
    }
    #endregion

    #region Private Methods
    void ObtainedOldManMeds() => _hasOldManMeds = true;
    bool HasOldManMeds() => _hasOldManMeds;

    void ObtainedSupplies() => _hasSupplies = true;
    bool HasSupplies() => _hasSupplies;

    void ObtainedCarKeys() => _hasCarKeys = true;
    bool HasCarKeys() => _hasCarKeys;

    void ObtainedManagerRoomKeys() => _hasManagerRoomKeys = true;
    bool HasManagerRoomKeys() => _hasManagerRoomKeys;

    void CompleteObjective(ObjectiveCompletion objectiveToComplete)
    {

    }

    void OnCompleteAssignedOldManTask(Notification notif)
    {
        UpdateObjectivePage(1, 0, true);
    }

    void OnCompleteAssignSuperMarketOwnerTask(Notification notif)
    {
        UpdateObjectivePage(5, 0, false);
    }

    void OnCompleteObtainOldManMeds(Notification notif)
    {
        ObtainedCarKeys();
        UpdateObjectivePage(3, 1, false);
    }

    void OnCompleteObtainSuppliesFromSuperMarket(Notification notif)
    {
        ObtainedSupplies();
    }

    void OnCompleteObtainedManagerRoomKeys(Notification notif)
    {
        ObtainedManagerRoomKeys();
    }

    void OnCompleteObtainedOldManMeds(Notification notif)
    {
        ObtainedOldManMeds();
        UpdateObjectivePage(2, 1, true);
    }


    #endregion

    #region Public Methods


    public void UpdateObjectivePage(int ObjectiveID, int page, bool IsShouldDoObjective)
    {
        //clearing previous objective
        _objectiveData.StrikeOutPreviousObjective(ObjectiveID);


        if(IsShouldDoObjective) _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[ObjectiveID], page);
        else _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[ObjectiveID], page);

    }


    //Objective Completion results
    public bool CheckConditionForObjectiveCompletion(ObjectiveCompletion objectiveToComplete, Notification notification, ObjectiveCompletionPrerequisite objectivePrerequisite)
    {
        if(ObjectiveCompletionPrerequisite.None == objectivePrerequisite)
        {
            UIManager.Instance.TriggerNotification(notification);
            CompleteObjective(objectiveToComplete);
        }
        else
        {
            switch(objectivePrerequisite)
            {
                case ObjectiveCompletionPrerequisite.ShouldHaveOldManMeds:
                    if (HasOldManMeds())
                    {
                        CompleteObjective(objectiveToComplete);
                    }
                    else return false;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveCarKeys:
                    if (HasCarKeys())
                    {
                        CompleteObjective(objectiveToComplete);
                    }
                    else return false;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveManagerRoomKeys:
                    if (HasManagerRoomKeys())
                    {
                        CompleteObjective(objectiveToComplete);
                    }
                    else return false;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveSupplies:
                    if (HasSupplies())
                    {
                        CompleteObjective(objectiveToComplete);
                    }
                    else return false;
                    break;
            }
        }

        return true;
    }

   
    #endregion
}
