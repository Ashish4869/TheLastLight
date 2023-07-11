using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This holds the objectives that we can currently see in the game.
/// </summary>

public class ObjectivePage : MonoBehaviour
{
    #region Variables
    [Header("Section References")]
    [SerializeField] TextMeshProUGUI _mainObjective;
    [SerializeField] TextMeshProUGUI _sideObjective;

    [Header("Page Content")]
    [SerializeField] [TextArea(3,5)] List<string> _mainObjectiveContent;
    [SerializeField] [TextArea(3, 5)] List<string> _sideObjectiveContent;

    [Header("NavigationButtons")]
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _prevButton;

    int _currentPage = 0, _totalNumberOfPages = 0;
    bool _nextButtonStatus = false, _prevButtonStatus = false;
    #endregion

    #region Monobehaviour CallBacks
   
    #endregion

    #region Private Methods
    void UpdatePage()
    {
        //Debug.Log("Current pages : " + _currentPage + " Total No of pages :" + _totalNumberOfPages);

        //Updating Section Content as per Data and page no
        _totalNumberOfPages = _mainObjectiveContent.Count > _sideObjectiveContent.Count ? _mainObjectiveContent.Count : _sideObjectiveContent.Count;
        _mainObjective.text = _currentPage < _mainObjectiveContent.Count ? _mainObjectiveContent[_currentPage] : "";
        _sideObjective.text = _currentPage < _sideObjectiveContent.Count ? _sideObjectiveContent[_currentPage] : "";

        //Checking Navigation Buttons Status
        if (_currentPage == (_totalNumberOfPages - 1) ) _nextButtonStatus = false; //we are at the last page
        else _nextButtonStatus = true;

        if (_currentPage == 0) _prevButtonStatus = false;//we are at the first page
        else _prevButtonStatus = true;


        _nextButton.SetActive(_nextButtonStatus);
        _prevButton.SetActive(_prevButtonStatus);
    }

    void UpdateParticularSectionData(List<string> SectionData,string updatedObjective, int PageNo)
    {
        if (PageNo < SectionData.Count)
        {
            SectionData[PageNo] = updatedObjective;
        }
        else
        {
            SectionData.Add(updatedObjective);
            _totalNumberOfPages++;
        }
    }
    #endregion

    #region Public Methods
    public void TurnToNexPage()
    {
        _currentPage++;
        UpdatePage();
    }

    public void TurnToPrevPage()
    {
        _currentPage--;
        UpdatePage();
    }

    public void MainObjectiveDataUpdate(string updatedObjective, int PageNo)
    {
        UpdateParticularSectionData(_mainObjectiveContent, updatedObjective, PageNo);
        UpdatePage();
    }

    public void SideObjectiveDataUpdate(string updatedObjective, int PageNo) 
    {
        UpdateParticularSectionData(_sideObjectiveContent, updatedObjective, PageNo);
        UpdatePage();
    }

    
  
    #endregion

}
