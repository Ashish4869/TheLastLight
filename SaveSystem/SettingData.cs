using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds data of the game configuration
/// </summary>

[System.Serializable]
public class SettingData
{
    public int _graphicsQuality;
    public bool _postProcessingBool;
    public float _gameVolume;
    public SettingData(SettingManager settings)
    {
        _graphicsQuality = settings.GetGraphicsQuality();
        _postProcessingBool = settings.GetPostProcessingBool();
        _gameVolume = settings.GetGameVolume();
    }


}
