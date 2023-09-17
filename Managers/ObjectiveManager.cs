using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System;

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
        ObtainManagerRoomKeys,

        //Level 2
        //Main
        ObtainFoodAndWater,
        ObtainPlaceToStay,

        //Side
        ObtainShotGun
    }

    public enum ObjectiveCompletionPrerequisite
    {
        //Level 1
        None,
        ShouldHaveOldManMeds,
        ShouldHaveSupplies,
        ShouldHaveCarKeys,
        ShouldHaveManagerRoomKeys,

        //Level 2
        ShouldHaveFoodWater
    }

    #region Variables
    //Level 1
    bool _hasOldManMeds = false;
    bool _hasSupplies = false;
    bool _hasCarKeys = false;
    bool _hasManagerRoomKeys = false;

    //Level 2
    bool _hasFoodAndWater = false;

    //for storing in disk
    bool[] _objectiveStatus;

    ObjectivePage _ObjectivePage;
    ObjectiveData _objectiveData = new ObjectiveData();
    #endregion

    #region MonoBehaviour CallBacks
    private void Awake()
    {
        _ObjectivePage = FindAnyObjectByType<ObjectivePage>();
        EventManager.OnCheckPointReached += SaveObjectiveStatus;
    }

    
    private void Start()
    {
        int level = SceneManager.GetActiveScene().buildIndex;

        switch (level)
        {
            case 0:
                _objectiveStatus = new bool[7];
                _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[0], 0);
                _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[4], 0);
                break;

            case 1:
                _objectiveStatus = new bool[3];
                _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[8], 0);      
                _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[10], 0);
                break;

            case 2:
                _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[12], 0);
                _ObjectivePage.SideObjectiveDataUpdate("", 0);
                break;
        }


        if(GameManager.Instance.HasValueFromDisk()) SetUpValuesFromDisk();
    }

    

    private void OnDestroy()
    {
        EventManager.OnCheckPointReached -= SaveObjectiveStatus;
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

    void ObtainedFoodAndWater() => _hasFoodAndWater = true;
    bool HasFoodWater() => _hasFoodAndWater;

    void CompleteObjective(ObjectiveCompletion objectiveToComplete)
    {
        switch(objectiveToComplete)
        {
            //Main
            //Level 1
            case ObjectiveCompletion.ReceiveTaskFromOldMan:
                OnCompleteAssignedOldManTask();
                _objectiveStatus[0] = true;
                break;

            case ObjectiveCompletion.ObtainMedsForOldMan:
                _objectiveStatus[1] = true;
                OnCompleteObtainedOldManMeds();
                break;

            case ObjectiveCompletion.ObtainOldManCarKeys:
                _objectiveStatus[2] = true;
                OnCompleteObtainedCarKeys();
                break;

            case ObjectiveCompletion.LeaveWellington:
                _objectiveStatus[3] = true;
                OnCompleteLeaveWellington();
                break;

            //Level 2
            case ObjectiveCompletion.ObtainFoodAndWater:
                _objectiveStatus[0] = true;
                OnCompleteObtainedFoodAndWater();
                break;

            case ObjectiveCompletion.ObtainPlaceToStay:
                _objectiveStatus[1] = true;
                OnCompletePlaceToStay();
                break;


            //Side
            //Level 1
            case ObjectiveCompletion.ReceiveTaskFromSuperMarketOwner:
                _objectiveStatus[4] = true;
                OnCompleteAssignSuperMarketOwnerTask();
                break;

            case ObjectiveCompletion.ObtainSuppliesFromSuperMarket:
                _objectiveStatus[5] = true;
                OnCompleteObtainSuppliesFromSuperMarket();
                break;

            case ObjectiveCompletion.ObtainManagerRoomKeys:
                _objectiveStatus[6] = true;
                OnCompleteObtainedManagerRoomKeys();
                    break;

             //Level 2
            case ObjectiveCompletion.ObtainShotGun:
                _objectiveStatus[2] = true;
                OnCompleteObtainedShotGun();
                break;
        }
    }

    private void OnCompletePlaceToStay()
    {
        GameManager.Instance.PlayCutscene();
    }

    private void OnCompleteObtainedFoodAndWater()
    {
        ObtainedFoodAndWater();
        UpdateObjectivePage(9,0,true);
    }

    private void OnCompleteObtainedShotGun()
    {
        UpdateObjectivePage(11, 0, false);
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

    private void SaveObjectiveStatus()
    {
        SaveData.Instance.SetObjectiveStatus(_objectiveStatus);
    }

    private void SetUpValuesFromDisk()
    {
        bool[] objectiveStatus = SaveData.Instance.GetObjectiveStatus();

        if (objectiveStatus == null) return;

        int level = SceneManager.GetActiveScene().buildIndex;
        
        switch(level)
        {
            case 0:
                for(int i = 0; i < 7; i++)
                {
                    if(objectiveStatus[i] == true)
                    {
                        ObjectiveCompletion objective = (ObjectiveCompletion)i;
                        CompleteObjective(objective);
                    }
                }
                break;

                
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    if (objectiveStatus[i] == true)
                    {
                        ObjectiveCompletion objective = (ObjectiveCompletion)(i+7);
                        CompleteObjective(objective);
                    }
                }
                break;
                

        }
    }

    #endregion

    #region Public Methods


    public void UpdateObjectivePage(int ObjectiveIndex, int page, bool IsMainObjective)
    {
        if(IsMainObjective) _ObjectivePage.MainObjectiveDataUpdate(_objectiveData.Data[ObjectiveIndex], page);
        else _ObjectivePage.SideObjectiveDataUpdate(_objectiveData.Data[ObjectiveIndex], page);
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
                //Level 1
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

                //Level 2
                case ObjectiveCompletionPrerequisite.ShouldHaveFoodWater:
                    if (HasFoodWater()) CanRunObjective = true;
                    break;
            }
        }

        if (CanRunObjective)
        {
            UIManager.Instance.TriggerNotification(notification);
            CompleteObjective(objectiveToComplete);
            FindAnyObjectByType<EventManager>().OnCheckPointReachedEvent();
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
