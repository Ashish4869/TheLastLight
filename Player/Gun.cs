using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Variables
    GameObject _currentWeapon;
    Transform PlayerEyes;
    Transform _currentMuzzleFlashPos;

    public GameObject BulletHolePrefab;
    public GameObject PistolMuzzleFlash;
    public GameObject ShotgunMuzzleFlash;
    public GameObject HitMarkerPrefab;
    public Weapon[] _loadout;
    [HideInInspector] public Weapon CurrentGunData;
    public Transform _weaponParent;
    public LayerMask _CanBeShot, _enemy, _destructable;
    public float CurrentCooldown;
    public GameObject ReloadingText;
    public bool _isAiming = false;

    bool _isReloading, _cantShoot, _firstBullet, _isSprinting;
    bool _OneGetMouseButtonUp;
    int _currentIndex = 99;
    bool _HasAK47 = false, _HasShotGun = false;
    float _recoil;
    #endregion

    #region MonoBehavivourCallBacks
    // Start is called before the first frame update
    void Start()
    {

        foreach (Weapon gun in _loadout)
        {
            gun.Initialise();
        }

        PlayerEyes = GameObject.Find("PlayerHead/PlayerEyes").GetComponent<Transform>();
        EquipGun(1);

        EventManager.OnPlayerDeath += Die;
    }

   

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GamePaused()) return;

        ChooseGun();
        WeaponFireMechanics();
        RefreshUI();
    }


    IEnumerator Reload(float relaodTime)
    {
        _isReloading = true;

        // _currentWeapon.SetActive(false);
        ReloadingText.SetActive(true);
        _currentWeapon.GetComponent<Animator>().Play("Reload", 0, 0);

        yield return new WaitForSeconds(relaodTime);

        //Prevents relaoding if weapon Switched
        if(_isReloading)
        {
            _loadout[_currentIndex].Reload();
        }
        
        // _currentWeapon.SetActive(true);
        ReloadingText.SetActive(false);
        _isReloading = false;

        _firstBullet = true;
    }

    IEnumerator ShowMuzzleFlash()
    {
        PistolMuzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        PistolMuzzleFlash.SetActive(false);
    }

    IEnumerator ShowShotGunFlash()
    {
        ShotgunMuzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        ShotgunMuzzleFlash.SetActive(false);
    }

    IEnumerator CantShootWhileEquiping(float equiptime)
    {
        _cantShoot = true;

        yield return new WaitForSeconds(equiptime);

        _cantShoot = false;
    }

    IEnumerator SetFirstBulletAccuracy()
    {
        yield return new WaitForSeconds(0.5f);
        _firstBullet = true;
        
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= Die;
    }
    #endregion

    #region Private Methods

    void Die()
    {
        enabled = false;
    }

    void ChooseGun()
    {
        //equip Axe
        if (Input.GetKey(KeyCode.Alpha0))
        {
            EquipGun(0);
        }

        //equip pistol
        if (Input.GetKey(KeyCode.Alpha1))
        {
            EquipGun(1);
        }

        //equip Ak-47
        if (Input.GetKey(KeyCode.Alpha2) && _HasAK47)
        {
            EquipGun(2);
        }

        //equip Shotgun
        if (Input.GetKey(KeyCode.Alpha3) && _HasShotGun)
        {
            EquipGun(3);
        }
    }


    void WeaponFireMechanics()
    {
        //execute only if we have weapon in hand
        if (_currentWeapon != null)
        {
            //if current weapon is Axe
            if (_loadout[_currentIndex].gun == Weapon.Gun.Axe)
            {
                if (Input.GetMouseButtonDown(0) && CurrentCooldown <= 0 && !_cantShoot && !_isSprinting)
                {
                    //play attack/swing animations
                    AxeAttack();
                }
            }


            //if pistol or shotgun
            if (_loadout[_currentIndex].gun == Weapon.Gun.Pistol || _loadout[_currentIndex].gun == Weapon.Gun.ShotGun)
            {
                if (Input.GetMouseButtonDown(0) && CurrentCooldown <= 0 && !_isReloading && !_cantShoot && !_isSprinting)
                {

                    if (_loadout[_currentIndex].FireBullet())
                    {
                        Shoot();
                    }
                    else
                    {
                        AudioManager.Instance.PlaySFX("EmptyGun");
                    }

                }

            }
            else
            {
                //if ak-47
                if (Input.GetMouseButton(0) && CurrentCooldown <= 0 && !_isReloading && !_cantShoot && _loadout[_currentIndex].gun == Weapon.Gun.AK47 && !_isSprinting)
                {

                    if (_loadout[_currentIndex].FireBullet())
                    {
                        Shoot();
                        PistolMuzzleFlash.transform.position = _currentMuzzleFlashPos.position;
                        PistolMuzzleFlash.SetActive(true);
                        if (_isAiming)
                        {
                            _recoil += 7.5f;
                        }
                        else
                        {
                            _recoil += 15f;
                        }

                    }
                    else
                    {
                        AudioManager.Instance.PlaySFX("EmptyGun");
                    }

                    _OneGetMouseButtonUp = false;

                }

                if ((Input.GetMouseButtonUp(0) || _loadout[_currentIndex].IsEmpty() || _isReloading) && !_OneGetMouseButtonUp)
                {
                    PistolMuzzleFlash.SetActive(false);
                    _recoil = 0f;
                    _OneGetMouseButtonUp = true;
                    StartCoroutine(SetFirstBulletAccuracy());
                }
            }

            //Weapon Position Elasicity
            _currentWeapon.transform.localPosition = Vector3.Lerp(_currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 3f);

            //Reload Button Pressed
            if (Input.GetKeyDown(KeyCode.R) && _isReloading == false && _loadout[_currentIndex].gun != Weapon.Gun.Axe) //reload if we are already reloading and not holding an axe
            {
                if (_loadout[_currentIndex].IsFull() == false && _loadout[_currentIndex].StashEmpty() == false)
                {
                    PlaySoundEffect(_loadout[_currentIndex].gun, "Reload");
                    StartCoroutine(Reload(_loadout[_currentIndex].ReloadTime));
                }
            }

        }

        //Cooldown setting
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= Time.deltaTime;
        }
    }

    private void PlaySoundEffect(Weapon.Gun gun, string type)
    {
        switch (gun)
        {
            case Weapon.Gun.Pistol:
                AudioManager.Instance.PlaySFX("Pistol" + type);
                break;

            case Weapon.Gun.AK47:
                AudioManager.Instance.PlaySFX("AK47" + type);
                break;

            case Weapon.Gun.ShotGun:
                AudioManager.Instance.PlaySFX("ShotGun" + type);
                break;

            case Weapon.Gun.Axe:
                AudioManager.Instance.PlaySFX("Axe" + type);
                break;
        }
    }
    

    void RefreshUI()
    {
        //refresh UI if not axe
        if (_loadout[_currentIndex].gun != Weapon.Gun.Axe)
        {
            UpdateUI();
            UpdateAmmoBarUI();
        }
    }

    void EquipGun(int ind)
    {
        // making sure the effects not turned on when equiping new gun
        PistolMuzzleFlash.SetActive(false);

        //if the we are trying to equip the same gun in hand then just return
        if(_currentIndex == ind)
        {
            return;
        }

        if(_currentWeapon != null)
        {
            if(_isReloading)
            {
                AudioManager.Instance.StopPlayingAudio(_loadout[_currentIndex].Name + "Reload");
                StopCoroutine("Reload");
            }

            Destroy(_currentWeapon);
        }

        ReloadingText.SetActive(false);
        _currentIndex = ind;
        GameObject NewWeapon = Instantiate(_loadout[_currentIndex].prefab, _weaponParent.position, _weaponParent.rotation , _weaponParent);
        NewWeapon.transform.localPosition = Vector3.zero;
        NewWeapon.transform.localEulerAngles = Vector3.zero;

        NewWeapon.GetComponent<Animator>().Play("Equip", 0, 0);
        _currentWeapon = NewWeapon;
        CurrentGunData = _loadout[ind];
        StartCoroutine(CantShootWhileEquiping(CurrentGunData.EquipTime));
        _isReloading = false;
        _firstBullet = true;

        PlaySoundEffect(_loadout[_currentIndex].gun, "Equip");

        //Show the CrossHair if the gun is not Axe
        if (CurrentGunData.gun != Weapon.Gun.Axe)
        {
            ShowCrossHair();
        }

        if (_loadout[_currentIndex].gun != Weapon.Gun.Axe)
        {
            _currentMuzzleFlashPos = _currentWeapon.transform.Find("Anchor/Resources/MuzzleFlashPos").GetComponent<Transform>();
        }

        UIManager.Instance.GunHighLight(_loadout[_currentIndex].gun); //highlights the selected gun

        if (_loadout[_currentIndex].gun == Weapon.Gun.Axe) //makes the ammo in visible if axe is held
        {
            UIManager.Instance.AmmoVisibility(true);
        }
        else
        {
            UIManager.Instance.AmmoVisibility(false);
        }
    }

    public bool Aim(bool isAiming)
    {
        if (_currentWeapon == null)
        {
            return false;
        }

        if(_isReloading)
        {
            isAiming = false;
        }
            
        _isAiming = isAiming;

        if(_currentWeapon != null && _loadout[_currentIndex].gun != Weapon.Gun.Axe)
        {
            Transform Anchor = _currentWeapon.transform.Find("Anchor");
            Transform HipState = _currentWeapon.transform.Find("States/Hip");
            Transform ADSState = _currentWeapon.transform.Find("States/ADS");

            if (isAiming)
            {
                Anchor.position = Vector3.Lerp(Anchor.position, ADSState.position, Time.deltaTime * _loadout[_currentIndex].aimSpeed);
                Anchor.rotation = Quaternion.Lerp(Anchor.rotation, ADSState.rotation, Time.deltaTime * _loadout[_currentIndex].aimSpeed);
                _loadout[_currentIndex].ADSShift(true);
            }
            else
            {
                Anchor.position = Vector3.Lerp(Anchor.position, HipState.position, Time.deltaTime * _loadout[_currentIndex].aimSpeed);
                Anchor.rotation = Quaternion.Lerp(Anchor.rotation, HipState.rotation, Time.deltaTime * 2f);
                _loadout[_currentIndex].ADSShift(false);
            }
        }

        return isAiming;
    }

    void AxeAttack()
    {
        int animNumber = Random.Range(1, 3);
        _currentWeapon.GetComponent<Animator>().Play("AttackAnim" + animNumber, 0, 0);
        SetCooldown();
    }

    void Shoot()
    {
        PlaySoundEffect(_loadout[_currentIndex].gun, "Shoot");
        GunFx();
        SetCooldown();

        //shoot
        for (int i = 0; i < Mathf.Max(1,CurrentGunData.pellets); i++)
        {
            Vector3 Spray = PlayerEyes.position + PlayerEyes.forward * 1000f;

            Spray = GetSpray(Spray);

            //shooting
            if (_currentWeapon != null)
            {
                RaycastHit hitinfo = new RaycastHit();
               

                //making the first bullet shot accurate
                if(_firstBullet)
                {
                    Spray = PlayerEyes.forward;
                }

                //Physics.SyncTransforms();

                //if we hit the enemy layer
                if (Physics.SphereCast(PlayerEyes.position, 0.2f, Spray, out hitinfo,  1000, _enemy)) //dont spawn a random bullet hole if we hit a enemy
                {
                    GameManager.Instance.ShowBloodParticleEffect(hitinfo);
                    hitinfo.transform.gameObject.GetComponentInParent<HealthManager>().TakeDamage(CurrentGunData.Damage , PlayerEyes.forward,hitinfo.rigidbody); //damage the enemy if shot
                }
                else if(Physics.Raycast(PlayerEyes.position, Spray, out hitinfo, 1000, _destructable) && i < 1) //destroy objects
                {
                    hitinfo.transform.gameObject.GetComponent<Destructible>().DestroyObject();
                }
                else if (Physics.Raycast(PlayerEyes.position, Spray, out hitinfo, 1000, _CanBeShot)) //spawn a random bullet hole if we dont hit a enemy
                {
                    GameObject BulletHole = Instantiate(BulletHolePrefab, hitinfo.point + hitinfo.normal * 0.001f , Quaternion.identity);
                    GameObject HitMarker = Instantiate(HitMarkerPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                    BulletHole.transform.LookAt(hitinfo.point + hitinfo.normal);
                    Destroy(BulletHole, 5f);
                    Destroy(HitMarker, 3f);

                    _firstBullet = false;
                } 
            }
        }

        //Give pump action if shotgun
        if(CurrentGunData.Recovery)
        {
            PlayShotGunPumpAnimation();
        }

        CheckifAlertedEnemy();
    }

   

    void GunFx()
    {
        //gunFx
        _currentWeapon.transform.Rotate(-_loadout[_currentIndex].Recoil, 0, 0);
        _currentWeapon.transform.position -= _currentWeapon.transform.forward * _loadout[_currentIndex].KickBack;
        if (_loadout[_currentIndex].gun == Weapon.Gun.Pistol)
        {
            PistolMuzzleFlash.transform.position = _currentMuzzleFlashPos.position;
            StartCoroutine(ShowMuzzleFlash());
        }

        if (_loadout[_currentIndex].gun == Weapon.Gun.ShotGun)
        {
            ShotgunMuzzleFlash.transform.position = _currentMuzzleFlashPos.position;
            StartCoroutine(ShowShotGunFlash());
        }

       
    }

    void SetCooldown() => CurrentCooldown = _loadout[_currentIndex].GetCurrentFireRate(); //Cooldown

    Vector3 GetSpray(Vector3 spray)
    {
        //Spray
        spray += Random.Range(-_loadout[_currentIndex].GetCurrentSpray() + _recoil, _loadout[_currentIndex].GetCurrentSpray() + _recoil) * PlayerEyes.up;
        spray += Random.Range(-_loadout[_currentIndex].GetCurrentSpray(), _loadout[_currentIndex].GetCurrentSpray()) * PlayerEyes.right;
        spray -= PlayerEyes.position;
        spray.Normalize();
        return spray;
    }


    void PlayShotGunPumpAnimation() => _currentWeapon.GetComponent<Animator>().Play("Recovery", 0, 0);

    void CheckifAlertedEnemy()
    {
        //creates a sphere around the gun which acts the range of sound 
        Collider[] Enemies = Physics.OverlapSphere(_weaponParent.position, _loadout[_currentIndex].SoundIntensity, _enemy); 

        for(int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].GetComponentInParent<EnemyAI>().SensedPlayer();
        }
    }


    private void ShowCrossHair() => UIManager.Instance.ShowCrosshair();
    #endregion


    #region Public Methods
    public void UpdateUI() => UIManager.Instance.UpdateAmmo(_loadout[_currentIndex].GetTotalCurrentAmmo(), _loadout[_currentIndex].GetClipAmmo());

    public void UpdateAmmoBarUI() => UIManager.Instance.UpdateAmmoBar(_loadout[_currentIndex].Ammo, _loadout[_currentIndex].GetCurrentAmmoBarValue());
    public void GotAK()
    {
        _HasAK47 = true;
        EquipGun(2);
    }

    public void GotShotGun()
    {
        _HasShotGun = true;
        EquipGun(3);
    }

    public void GotAmmo(Weapon.AmmoType Ammo, Notification notif)
    {
        int bullets = 0;
        switch(Ammo)
        {
            case Weapon.AmmoType.Pistol:
                bullets = Random.Range(15, 30);
                AudioManager.Instance.PlaySFX("PistolEquip");
                notif.SetNotificationString(bullets + "x Pistols Rounds");
                break;

            case Weapon.AmmoType.AK47:
                bullets = Random.Range(40, 120);
                notif.SetNotificationString(bullets + "x AK47 Rounds");
                AudioManager.Instance.PlaySFX("AK47Equip");
                break;

            case Weapon.AmmoType.Shotgun:
                bullets = Random.Range(6, 15);
                notif.SetNotificationString(bullets + "x Shotgun Rounds");
                AudioManager.Instance.PlaySFX("ShotGunEquip");
                break;   
        }

        _loadout[(int)Ammo].UpdateAmmo(bullets);
        UIManager.Instance.UpdateAmmoBar(Ammo, _loadout[(int)Ammo].GetCurrentAmmoBarValue());
        UIManager.Instance.TriggerNotification(notif);
    }

    public void EnableRunningInaccuracy() => _loadout[_currentIndex].RUNShift(true);
    public void DisableRunningAccuracy() => _loadout[_currentIndex].RUNShift(false);

    public void PlayerSprinting() => _isSprinting = true;

    public void PlayerNotSprinting() => _isSprinting = false;
    #endregion
}
