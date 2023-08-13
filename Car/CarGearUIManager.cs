using UnityEngine;

/// <summary>
/// Manages the UI for the gears.
/// </summary>
public class CarGearUIManager : MonoBehaviour
{
    [SerializeField] RectTransform _gearStick;
    [SerializeField] RectTransform _neutral;
    [SerializeField] RectTransform _high;

    public void UpdateGearUI(bool IsInHighGear)
    {
        if (!IsInHighGear) _gearStick.position = _neutral.position;
        else _gearStick.position = _high.position;
    }
}
