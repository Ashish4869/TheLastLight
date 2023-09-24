using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;
using System;

public class SettingManager : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] TMP_Dropdown _graphicsDropdown;
    [SerializeField] Toggle _postprocessingToggle;
    [SerializeField] Slider _volumeSlider;

    int _graphicsQuality;
    bool _postProcessBool;
    float _gameVolume;

    #region Monobehaviour Callbacks
    void OnEnable()
    {
        LoadDataFromDisk();
    }

    private void Start()
    {
        _graphicsDropdown.value = 5; //ultra
        _postprocessingToggle.isOn = true;
    }

    #endregion

    #region Private Methods
    private void LoadDataFromDisk()
    {
        SettingData data = SaveSystem.LoadSettingData();

        if (data == null) return;

        _graphicsQuality = data._graphicsQuality;
        _postProcessBool = data._postProcessingBool;
        _gameVolume = data._gameVolume;
        SetUpSettingsUI();
    }

    void SetUpSettingsUI()
    {
        _graphicsDropdown.value = _graphicsQuality;
        _postprocessingToggle.isOn = _postProcessBool;
        _volumeSlider.value = _gameVolume;
    }
    #endregion

    #region Public Methods
    public void ChangeGraphicsQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        _graphicsQuality = quality;
    }

    public void ChangePostProcessing(bool shouldApplyPostProcessing)
    {
       FindAnyObjectByType<TogglePostProcessing>().TogglePostProcessingVolume(shouldApplyPostProcessing);
        _postProcessBool = shouldApplyPostProcessing;
    }

    public void ChangeVolumeSettings(float value)
    {
        mainMixer.SetFloat("volume", value);
        _gameVolume = value;
    }

    public void SaveDataIntoDisk()
    {
        SaveSystem.SaveSettingsData(this);
    }

    //getters
    public int GetGraphicsQuality() => _graphicsQuality;
    public bool GetPostProcessingBool() => _postProcessBool;
    public float GetGameVolume() => _gameVolume;
    #endregion
}
