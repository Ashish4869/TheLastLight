using System;
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
        //Main
        ReceiveTaskFromOldMan,
        ObtainMedsForOldMan,
        ObtainOldManCarKeys,
        LeaveWellington,

        //Side
        ReceiveTaskFromSuperMarketOwner,
        ObtainSuppliesFromSuperMarket,
        ObtainManagerRoomKeys
       
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
    public bool HasManagerRoomKeys() => _hasManagerRoomKeys;

    void CompleteObjective(ObjectiveCompletion objectiveToComplete)
    {
        switch(objectiveToComplete)
        {
            //Main
            case ObjectiveCompletion.ReceiveTaskFromOldMan:
                OnCompleteAssignedOldManTask();
                break;

            case ObjectiveCompletion.ObtainMedsForOldMan:
                OnCompleteObtainedOldManMeds();
                break;

            case ObjectiveCompletion.ObtainOldManCarKeys:
                OnCompleteObtainedCarKeys();
                break;

            case ObjectiveCompletion.LeaveWellington:
                OnCompleteLeaveWellington();
                break;

            //Side
            case ObjectiveCompletion.ReceiveTaskFromSuperMarketOwner:
                OnCompleteAssignSuperMarketOwnerTask();
                break;

            case ObjectiveCompletion.ObtainSuppliesFromSuperMarket:
                OnCompleteObtainSuppliesFromSuperMarket();
                break;

            case ObjectiveCompletion.ObtainManagerRoomKeys:
                OnCompleteObtainedManagerRoomKeys();
                    break;
        }
    }

    private void OnCompleteLeaveWellington()
    {
        UIManager.Instance.HideUI();
        GameManager.Instance.PlayCutscene();
    }

    private void OnCompleteObtainedCarKeys()
    {
        ObtainedCarKeys();
        UpdateObjectivePage(3, 1, true);
    }

    void OnCompleteAssignedOldManTask()
    {
        UpdateObjectivePage(1, 0, true);
    }

    void OnCompleteAssignSuperMarketOwnerTask()
    {
        UpdateObjectivePage(5, 0, false);
    }

   
    void OnCompleteObtainSuppliesFromSuperMarket()
    {
        ObtainedSupplies();
        StrikeOutPreviousPage(5); //strike out page
        _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[5], 0); //update page
        UpdateObjectivePage(6, 1, false);
    }

    void OnCompleteObtainedManagerRoomKeys()
    {
        ObtainedManagerRoomKeys();
        UpdateObjectivePage(7, 1, false);
    }

    void OnCompleteObtainedOldManMeds()
    {
        ObtainedOldManMeds();
        StrikeOutPreviousPage(1); //strike out page
        _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[1], 0); //update page
        UpdateObjectivePage(2, 1, true);
    }

    

    private void StrikeOutPreviousPage(int PageNo)
    {
        _objectiveData.Data[PageNo] = "<s>" + _objectiveData.Data[PageNo] + "</s>";
    }
    #endregion

    #region Public Methods


    public void UpdateObjectivePage(int ObjectiveID, int page, bool IsShouldDoObjective)
    {
        if(IsShouldDoObjective) _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[ObjectiveID], page);
        else _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[ObjectiveID], page);
    }

    //Objective Completion results
    public bool CheckConditionForObjectiveCompletion(ObjectiveCompletion objectiveToComplete, Notification notification, ObjectiveCompletionPrerequisite objectivePrerequisite)
    {
        bool CanRunObjective = false;
        if(ObjectiveCompletionPrerequisite.None == objectivePrerequisite)
        {
            CanRunObjective = true;
        }
        else
        {
            switch(objectivePrerequisite) //check prerequisites
            {
                case ObjectiveCompletionPrerequisite.ShouldHaveOldManMeds:
                    if (HasOldManMeds()) CanRunObjective = true;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveCarKeys:
                    if (HasCarKeys()) CanRunObjective = true;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveManagerRoomKeys:
                    if (HasManagerRoomKeys()) CanRunObjective = true;
                    break;

                case ObjectiveCompletionPrerequisite.ShouldHaveSupplies:
                    if (HasSupplies()) CanRunObjective = true;
                    break;
            }
        }

        if (CanRunObjective)
        {
            UIManager.Instance.TriggerNotification(notification);
            CompleteObjective(objectiveToComplete);
        }

        return CanRunObjective;
    }


    public void OnCompleteVisitingManagersRoom()
    {
        StrikeOutPreviousPage(6); //strike out page
        _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[6], 0); //update page
    }

    #endregion
}
