
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
        GameManager.Instance.CutSceneFinished();
    }
    #endregion

    #region Public Methods
    public void PlayCutscene()  
    {
        _videoPlayer.enabled = true;
        _cutSceneCamera.enabled = true; 
        _videoPlayer.clip = _cutscenes[_currentCutsceneIndex++];
    }

    public void StartCutscene()
    {
        SetupCutscene();
        PlayCutscene();
    }

    #endregion



}
