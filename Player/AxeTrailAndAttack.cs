using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// suports the functionality of turning off and on the trail renderer at the specific time of animation
/// allows public methods for disableing the attack point
/// </summary>
public class AxeTrailAndAttack : MonoBehaviour
{
    TrailRenderer _axeTrail;
    [SerializeField]
    private GameObject _axeAttackPoint;

    private void Awake()
    {
        _axeTrail = transform.Find("Anchor/Design/Fire_Axe_LODA/Trail").GetComponent<TrailRenderer>(); //Get the trail Renderer
    }
    public void ActivateTrail() => _axeTrail.enabled = true;
    public void PlayAxeEffects() => AudioManager.Instance.PlaySFX("AxeShoot");

    public void DeactivateTrail() => _axeTrail.enabled = false;

    public void SetAttackPointActive() => _axeAttackPoint.SetActive(true);

    public void SetAttackPointInActive() => _axeAttackPoint.SetActive(false);

}
