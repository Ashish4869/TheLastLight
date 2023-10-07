using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.Video;
using System.Collections;

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
            AudioManager.Instance.StopPlayingAudio("HeavyBreathing");
            AudioManager.Instance.StopPlayingAudio("HeartPounding");
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

        //Harding coding values for Level 3, can't help it :(
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            StartCoroutine(StartCutsceneLittleLater());
            return;
        }

        _videoPlayer.enabled = true;
        _cutSceneCamera.enabled = true; 
        _videoPlayer.clip = _cutscenes[_currentCutsceneIndex++];
        SaveData.Instance.SetCutsceneIndex(_currentCutsceneIndex);
    }

    IEnumerator StartCutsceneLittleLater()
    {
        yield return new WaitForSeconds(0.25f);
       

        _currentCutsceneIndex = 0;

        if (SaveData.Instance.GetIsLevel3ZombiesDead())
        {
            _currentCutsceneIndex = 1;
        }

        if (SaveData.Instance.GetIsLevel3BossDead())
        {
            Debug.Log("Play last cutscene");
            _currentCutsceneIndex = 2;
        }

       

        _videoPlayer.enabled = true;
        _cutSceneCamera.enabled = true;
        _videoPlayer.clip = _cutscenes[_currentCutsceneIndex++];
        SaveData.Instance.SetCutsceneIndex(_currentCutsceneIndex);
    }







    #endregion



}
