
using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages the cutscenes to be played
/// </summary>
public class CutSceneManager : MonoBehaviour
{
    #region Variables
    VideoPlayer _videoPlayer;
    [SerializeField] VideoClip[] _cutscenes;
    int _currentCutsceneIndex = 0;
    [SerializeField] Camera _cutSceneCamera;
    #endregion

    #region Monobehaviour Callbacks
    private void Awake()
    {
        SetupCutscene();
        SetupValueFromDisk();
    }

    private void SetupValueFromDisk()
    {
        if (GameManager.Instance.HasValueFromDisk()) _currentCutsceneIndex = SaveData.Instance.GetCutsceneIndex();
    }
    #endregion

    #region Private Methods
    private void SetupCutscene()
    {
        _videoPlayer = (VideoPlayer)GetComponent("VideoPlayer");
        _videoPlayer.loopPointReached += CutSceneEnded;
    }
  
  
    void CutSceneEnded(VideoPlayer vp)
    {
        _videoPlayer.enabled = false;
        _cutSceneCamera.enabled = false;

        if(_currentCutsceneIndex == _cutscenes.Length)
        {
            GameManager.Instance.LoadlevelAfterCutscene();
        }
        else
        {
            GameManager.Instance.CutSceneFinished();

        }
       
    }
    #endregion

    #region Public Methods
    public void PlayCutscene()  
    {
        if (!GameManager.Instance.CanPlayCutscene())
        {
            return;
        }

        _videoPlayer.enabled = true;
        _cutSceneCamera.enabled = true; 
        _videoPlayer.clip = _cutscenes[_currentCutsceneIndex++];
        SaveData.Instance.SetCutsceneIndex(_currentCutsceneIndex);
    }

    

   

    #endregion



}
