
using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages the cutscenes to be played
/// </summary>
public class CutSceneManager : MonoBehaviour
{
    #region Variables
    VideoPlayer _vidoePlayer;
    [SerializeField] VideoClip[] _cutscenes;
    bool _isCutscenePlaying = true;
    int _currentCutsceneIndex = 0;
    [SerializeField] Camera _cutSceneCamera;
    #endregion

    #region Monobehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        SetupCutscene();
        PlayCutscene();
    }

    private void SetupCutscene()
    {
        _vidoePlayer = (VideoPlayer)GetComponent("VideoPlayer");
        _vidoePlayer.loopPointReached += CutSceneEnded;
        GameManager.Instance.StartCutscene();
    }
    #endregion

    #region Private Methods
    void CutSceneEnded(VideoPlayer vp)
    {
        _vidoePlayer.enabled = false;
        _cutSceneCamera.enabled = false;
        _isCutscenePlaying = false;
        GameManager.Instance.CutSceneFinished();
    }
    #endregion

    #region Public Methods
    public void PlayCutscene()  
    {
        _vidoePlayer.enabled = true;
        _cutSceneCamera.enabled = true; 
        _vidoePlayer.clip = _cutscenes[_currentCutsceneIndex++];
    }

    public bool IsCutScenePlaying() => _isCutscenePlaying;
   
    #endregion



}
