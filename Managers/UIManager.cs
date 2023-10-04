using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    #region Singleton Pattern
    public static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
               _instance = FindAnyObjectByType<UIManager>();

                if (_instance == null) _instance = new GameObject().AddComponent<UIManager>();
            }

            return _instance;
        }
    }

    #endregion

    #region Variables
    [Header("LoadOut")]
    [SerializeField] GameObject AKUI;
    [SerializeField] GameObject ShotGunUI;
    [SerializeField] GameObject _PistolHighligth;
    [SerializeField] GameObject _AKlight;
    [SerializeField] GameObject _ShotGunHighLight;
    [SerializeField] GameObject _AxeLight;

    [Header("Other UI elements")]
    [SerializeField] GameObject _CrossHair;
    [SerializeField] GameObject _NoAmmo;
    [SerializeField] Image _damageOverlay;
    [SerializeField] Animator _criticalHealth;
    [SerializeField] GameObject _DeathUI;
    [SerializeField] TextMeshProUGUI _promptMessage;
    [SerializeField] GameObject _DialougueBox;
    [SerializeField] GameObject _continueBox;
    [SerializeField] GameObject _ButtonPrompt;
    [SerializeField] GameObject _notificationParent;
    [SerializeField] Animator _Objective;
    [SerializeField] GameObject _LoadOutParent;
    [SerializeField] GameObject _interactivePromptMessage;
    [SerializeField] GameObject _carGearUI;
    [SerializeField] Animator _saveAnimation;

    [SerializeField] GameObject _notificationPrefab;
    TextMeshProUGUI _speaker, _dialouge;
    Slider _PistolSlider, _AKSlider, _ShotGunSLider;

    bool _isCritical = true, _showObjective = false,  _isInCutscene = true;
    bool _IsdialougeAnimating = false;
    public float _damageTimer = 2f; 
    #endregion


    #region MonoBehaviour CallBacks
    
    void Awake()
    {
        _PistolHighligth.SetActive(true);
        _PistolSlider = GameObject.Find("LoadOut/LoadOutBG/Pistol/AmmoBarPistol").GetComponent<Slider>();
        _AKSlider = GameObject.Find("LoadOut/LoadOutBG/AK47/AmmoBarAK47").GetComponent<Slider>();
        _ShotGunSLider = GameObject.Find("LoadOut/LoadOutBG/ShotGun/AmmoBarShotGun").GetComponent<Slider>();
        _speaker = _DialougueBox.transform.Find("DialougeBox/Speaker").GetComponent<TextMeshProUGUI>();
        _dialouge = _DialougueBox.transform.Find("DialougeBox/Dialouge").GetComponent<TextMeshProUGUI>();

        RegisterEvents();
    }

    private void Start()
    {
        SetUpValuesFromDisk();
    }

    // Update is called once per frame
    void Update()
    {
       if (_isInCutscene || GameManager.Instance.DialougeStatus()) return;

       if(Input.GetKeyDown(KeyCode.Tab))
       {
            ObjectiveBookToggle();
       }    
    }

    IEnumerator DeathUI()
    {
        yield return new WaitForSeconds(3f);
        _DeathUI.SetActive(true);
        FindObjectOfType<LockCursor>().UpdateCursorLock();
    }

    IEnumerator FadeOutDamageOverlay()
    {
        while (_damageTimer >= 0)
        {
            _damageTimer -= Time.deltaTime;
            _damageOverlay.color = new Color(1, 1, 1, _damageTimer);
            yield return null;
        }
    }

   
    IEnumerator AnimateDialouge(string dialouge)
    {
        _continueBox.SetActive(false);
        _IsdialougeAnimating = true;
        int CurrentCharacter = 0;
        _dialouge.text = "";

        while(CurrentCharacter < dialouge.Length)
        {
            _dialouge.text += dialouge[CurrentCharacter];
            yield return new WaitForSecondsRealtime(0.01f);
            CurrentCharacter++;
        }

        _IsdialougeAnimating = false;
        _continueBox.SetActive(true);
    }

    IEnumerator DisableCrossHairAfterSomeTime()
    {
        yield return new WaitForSeconds(0.5f);
        _CrossHair.SetActive(false);
    }
    #endregion


    #region Private Methods
    private void SetUpValuesFromDisk()
    {
        if(SaveData.Instance.GetAKBool()) AKUI.SetActive(true);
        else AKUI.SetActive(false);

        if (SaveData.Instance.GetShotGunBool()) ShotGunUI.SetActive(true);
        else ShotGunUI.SetActive(false);

        UpdateAmmoBar(Weapon.AmmoType.AK47, SaveData.Instance.GetAKBullets());
        UpdateAmmoBar(Weapon.AmmoType.Shotgun, SaveData.Instance.GetShotGunBullets());
    }

    private void RegisterEvents()
    {
        EventManager.OnPlayerDeath += DisableUI;
        EventManager.OnEndCutscene += EnableUIAfterCutscene;
        EventManager.OnStartCutscene += DisableUIBeforeCutscene;
        EventManager.OnCheckPointReached += ShowSaveAnimation;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= DisableUI;
        EventManager.OnEndCutscene -= EnableUIAfterCutscene;
        EventManager.OnStartCutscene -= DisableUIBeforeCutscene;
        EventManager.OnCheckPointReached -= ShowSaveAnimation;
    }

    private void DisableUI()
    {
        _CrossHair.SetActive(false);
        GameObject g = GameObject.Find("LoadOut");
        if (g) g.SetActive(false);

        StartCoroutine(DeathUI());
    }
    private void EnableUIAfterCutscene()
    {
        _isInCutscene = false;   
        _CrossHair.SetActive(true);
        _LoadOutParent.SetActive(true);
        _interactivePromptMessage.SetActive(true);
        _damageOverlay.color = new Color(1, 1, 1, 0);

        if (GameManager.Instance.GetCurrentLevel() == 2) SetUpUIForCar();
    }

    private void DisableUIBeforeCutscene()
    {
        _isInCutscene = true;
        _damageOverlay.color = new Color(1, 1, 1, 0);
        _criticalHealth.SetTrigger("NotCritical");
        _interactivePromptMessage.SetActive(false);
        _CrossHair.SetActive(false);
        _LoadOutParent.SetActive(false);
        _carGearUI.SetActive(false);
        StartCoroutine(DisableCrossHairAfterSomeTime());
    }

    

    #endregion

    #region Public Methods
    public void ReloadGame()
    {
        FindAnyObjectByType<LevelLoader>().LoadParticularLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        FindAnyObjectByType<LevelLoader>().LoadParticularLevel(0);
    }

    public void UpdateAmmo(int TotalAmmo, int ClipAmmo)
    {
        if (TotalAmmo == 0 && ClipAmmo == 0)
        {
            ShowNoAmmoUI();
        }
        else
        {
            HideNoAmmoUI();
        }
    }

    public void AmmoVisibility(bool axe)
    {
        if (axe)
        {
            HideNoAmmoUI();
        }
    }

    public void UpdateAmmoBar(Weapon.AmmoType Ammo, float SliderValue)
    {
        switch (Ammo)
        {
            case Weapon.AmmoType.Pistol:
                _PistolSlider.value = SliderValue;
                break;

            case Weapon.AmmoType.AK47:
                _AKSlider.value = SliderValue;
                break;

            case Weapon.AmmoType.Shotgun:
                _ShotGunSLider.value = SliderValue;
                break;
        }
    }

    public void ShowGun(Weapon.Gun Gun)
    {
        switch (Gun)
        {
            case Weapon.Gun.AK47:
                AKUI.SetActive(true);
                GunHighLight(Gun);
                break;

            case Weapon.Gun.ShotGun:
                ShotGunUI.SetActive(true);
                GunHighLight(Gun);
                break;
        }
    }

    public void GunHighLight(Weapon.Gun Gun)
    {
        switch (Gun)
        {
            case Weapon.Gun.Axe:
                _AxeLight.SetActive(true);
                _PistolHighligth.SetActive(false);
                _AKlight.SetActive(false);
                _ShotGunHighLight.SetActive(false);
                break;

            case Weapon.Gun.Pistol:
                _AxeLight.SetActive(false);
                _PistolHighligth.SetActive(true);
                _AKlight.SetActive(false);
                _ShotGunHighLight.SetActive(false);
                break;

            case Weapon.Gun.AK47:
                _AKlight.SetActive(true);
                _AxeLight.SetActive(false);
                _ShotGunHighLight.SetActive(false);
                _PistolHighligth.SetActive(false);
                break;

            case Weapon.Gun.ShotGun:
                _ShotGunHighLight.SetActive(true);
                _AxeLight.SetActive(false);
                _PistolHighligth.SetActive(false);
                _AKlight.SetActive(false);
                break;
        }
    }

    public void ShowDamageOverlay()
    {
        _damageTimer = 1f;
        _damageOverlay.color = new Color(1, 1, 1, _damageTimer);

        if (GameManager.Instance.GetPlayerHealth() < 35)
        {
            _criticalHealth.SetTrigger("Critical");

            if (_isCritical)
            {
                PlayCriticalSoundEffect();
                _isCritical = false;
            }

            StopCoroutine(FadeOutDamageOverlay());
            _damageOverlay.color = new Color(1, 1, 1, 1);
            return;
        }

        StartCoroutine(FadeOutDamageOverlay());
    }

    private void PlayCriticalSoundEffect()
    {
        AudioManager.Instance.FadeInAudio("HeavyBreathing");
        AudioManager.Instance.FadeInAudio("HeartPounding");
    }

    public void PickedUpMedPack()
    {
        _criticalHealth.SetTrigger("NotCritical");
        AudioManager.Instance.FadeOutAudio("HeavyBreathing");
        AudioManager.Instance.FadeOutAudio("HeartPounding");
        _isCritical = true;

        if (_damageOverlay.color.a > 0)
        {
            StartCoroutine(FadeOutDamageOverlay());
        }
    }

    public void HideCrossHair() => _CrossHair.SetActive(false);

    public void ShowCrosshair() => _CrossHair.SetActive(true);

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void SetUpDialougeUI()
    {
        _promptMessage.color = new Color(0, 0, 0, 0);
        _DialougueBox.SetActive(true);
    }

    public void DisableDialougeUI()
    {
        _promptMessage.color = new Color(1, 1, 1, 1);
        _DialougueBox.SetActive(false);
    }

    public void SetDialouge(string speaker, string dialouge)
    {
        _speaker.text = speaker;
        StartCoroutine(AnimateDialouge(dialouge));
    }

    
    public void TriggerNotification(Notification notif)
    {
        GameObject notification = Instantiate(_notificationPrefab, _notificationParent.transform);
        notification.GetComponent<SetupNotification>().SetUpNotification(notif, notif.GetNotificationString());
    }

    public void SetButtonPrompt(string messsage)
    {
        _ButtonPrompt.SetActive(true);
        _promptMessage.text = messsage;
    }

    public void ClearButtonPrompt()
    {
        _ButtonPrompt.SetActive(false);
        _promptMessage.text = "";
    }


    public void ShowNoAmmoUI() => _NoAmmo.SetActive(true);
    public void HideNoAmmoUI() => _NoAmmo.SetActive(false);

    public bool IsDialougeAnimating() => _IsdialougeAnimating;

    public void ObjectiveBookToggle()
    {
        AudioManager.Instance.PlaySFX("ToggleObjective");
        _showObjective = !_showObjective;
        _Objective.SetBool("ShowObjective", _showObjective);
        GameManager.Instance.SetGamePauseStatus(_showObjective);
        Time.timeScale = _showObjective ? 0 : 1;
        FindAnyObjectByType<LockCursor>().UpdateCursorLock();
    }


    //Car Mechanics
    public void SetUpUIForCar()
    {
        _LoadOutParent.SetActive(false);
        _interactivePromptMessage.SetActive(false);
        _CrossHair.SetActive(false);
        _carGearUI.SetActive(true);
    }

    public void EnableUIFromCar()
    {
        _LoadOutParent.SetActive(true);
        _interactivePromptMessage.SetActive(true);
        _CrossHair.SetActive(true);
        _carGearUI.SetActive(false);
    }


    //Saving Animation
    public void ShowSaveAnimation()
    {
        _saveAnimation.SetTrigger("ShowSaveAnimation");
    }

    //cutscene
    public void SetIsInCutscene(bool condition) => _isInCutscene = condition;
    #endregion
}
