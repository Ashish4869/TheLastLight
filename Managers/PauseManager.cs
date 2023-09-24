using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseManager : MonoBehaviour
{
    #region Variables
    public PostProcessVolume volume;
    ColorGrading _colorGrading;
    
    [SerializeField] GameObject _pauseUI;
    [SerializeField] GameObject _crossHairUI;

    bool _isInCutscene = false;
    #endregion

    #region MonoBehaviour Callbacks
    //Gets the color grading in the post processing volume
    private void Awake()
    {
        volume.profile.TryGetSettings(out _colorGrading);
        _colorGrading.postExposure.value = 0f;
        _colorGrading.saturation.value = 0;

        EventManager.OnStartCutscene += DisablePauseBeforeCutscene;
        EventManager.OnEndCutscene += EnablePauseAfterCutscene;
    }

    private void Update()
    {
        if (_isInCutscene || GameManager.Instance.DialougeStatus()) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _colorGrading.saturation.value = -100;
            _colorGrading.contrast.value = 50;
            _crossHairUI.SetActive(false);
            _pauseUI.SetActive(true);

            Time.timeScale = 0;
            GameManager.Instance.SetGamePauseStatus(true);
        }
    }
    #endregion

    #region Private Methods
    void EnablePauseAfterCutscene()
    {
        _isInCutscene = false;
    }

    void DisablePauseBeforeCutscene()
    {
        _isInCutscene = true;
    }
    #endregion

    #region Public Methods

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseUI.SetActive(false);
        _crossHairUI.SetActive(true);
        FindObjectOfType<LockCursor>().UpdateCursorLock();
        GameManager.Instance.SetGamePauseStatus(false);
        _colorGrading.saturation.value = 0;
        _colorGrading.contrast.value = 0;
    }

    public void MainMenu()
    {
        FindAnyObjectByType<LevelLoader>().LoadParticularLevel(0);
    }

    public void Quit() => Application.Quit();

    private void OnDisable()
    {
        EventManager.OnStartCutscene -= DisablePauseBeforeCutscene;
        EventManager.OnEndCutscene -= EnablePauseAfterCutscene;
    }
    #endregion


}
