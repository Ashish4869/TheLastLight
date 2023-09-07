using System.Collections;
using UnityEngine;

/// <summary>
/// 1.Handles movement
/// 2.Enemy Detection With Sound
/// 3.Head Bob
/// 4.Weapon PickUp
/// 5. Movement Sounds
/// </summary>

public class Player : MonoBehaviour
{
    #region Variables
    Rigidbody _playerRig;
    Vector3 _Direction;
    float _speed = 250f;
    float _normalSpeed = 2000f; //later change to 250
    float _runSpeed = 500f;
    float _slowWalkSpeed = 100f;
    float _NormalFOV;
    float _sprintFOVModifier = 1.5f;
    float _jumpForce = 1000f;
    float _sprintTimer = 5f;
    Camera _playerEyes;
    Camera _WeaponCam;
    Transform _weaponParent;
    Vector3 _TargetWeaponBobPostion, _originalWeaponPosition;
    float idleCounter, MovementCounter;
    Gun _weapon;
    float ADSHeadBobSens = 0;
    bool isAiming = false;
    bool _isLanded = false;
    bool _playerTired = false;
    bool _isInCutscene = false;

    private float _SlowWalkSoundRaduis = 1.5f;
    private float _walkSoundRaduis = 5f;
    private float _SprintSoundRaduis = 15f;
    private float _landSoundRaduis = 25f;

    //Sound related
    private float _walkStep = 1f, _runStep = 0.5f, _sprintStep = 0.25f, _stepTimer = 0;

    public LayerMask _ground, _enemy;
    public Transform _groundDetector;
    public Transform Anchor;
    public Transform HipPos;
    public Transform ADSPos;

    private SphereCollider _PlayerSphereCollider;
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        _playerRig = GetComponent<Rigidbody>();
        _weapon = GetComponent<Gun>();
        _playerEyes = GameObject.Find("PlayerHead/PlayerEyes").GetComponent<Camera>();
        _WeaponCam = GameObject.Find("PlayerHead/WeaponCam").GetComponent<Camera>();
        _weaponParent = GameObject.Find("Weapon").GetComponent<Transform>();    
        _PlayerSphereCollider = GetComponent<SphereCollider>();  
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += Die;
        EventManager.OnStartCutscene += DisablePlayerBeforeCutscene;
        EventManager.OnEndCutscene += EnablePlayerAfterCutscene;
    }

    // Start is called before the first frame update
    void Start()
    {
        _NormalFOV = _playerEyes.fieldOfView;
        _originalWeaponPosition = _weaponParent.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isInCutscene) return;
        //input
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");
        bool Run = Input.GetKey(KeyCode.LeftShift);
        bool Jump = Input.GetKeyDown(KeyCode.Space);
        bool Aim = Input.GetMouseButton(1);
        bool Slow = Input.GetKey(KeyCode.LeftControl);


        //States
        bool _isGrounded = Physics.Raycast(_groundDetector.position, Vector3.down, 0.5f, _ground);
        bool _CanJump = Jump && _isGrounded;
        bool _CanRun = Run && (VerticalMovement > 0) && !Jump && _isGrounded && (_sprintTimer > 0f);
        bool _CanSlow = _isGrounded && Slow && !Jump && !Run;
        isAiming = Aim && !_CanRun && !_CanJump;

        //Aiming 
        isAiming = _weapon.Aim(isAiming);

        //************Camera effects******************
        //Running
        if (_CanRun)
        {
            _speed = _runSpeed;
            _playerEyes.fieldOfView = Mathf.Lerp(_playerEyes.fieldOfView, _NormalFOV * _sprintFOVModifier, Time.deltaTime * 6f);
            _WeaponCam.fieldOfView = Mathf.Lerp(_WeaponCam.fieldOfView, _NormalFOV * _sprintFOVModifier, Time.deltaTime * 6f);
        }
        //ADS 
        else if (isAiming)
        {
            _playerEyes.fieldOfView = Mathf.Lerp(_playerEyes.fieldOfView, _NormalFOV * _weapon.CurrentGunData.MainCamFov, Time.deltaTime * 4f);
            _WeaponCam.fieldOfView = Mathf.Lerp(_WeaponCam.fieldOfView, _NormalFOV * _weapon.CurrentGunData.GunCamFov, Time.deltaTime * 3f);
            HideCrossHair();
        }
        //idle
        else
        {
            _playerEyes.fieldOfView = Mathf.Lerp(_playerEyes.fieldOfView, _NormalFOV, Time.deltaTime * 5f);
            _WeaponCam.fieldOfView = Mathf.Lerp(_WeaponCam.fieldOfView, _NormalFOV, Time.deltaTime * 4f);
            _speed = _normalSpeed;
        }

        //SlowWalking
        if (_CanSlow)
        {
            _speed = _slowWalkSpeed;
        }

        //Jumping
        if (_CanJump)
        {
            _playerRig.AddForce(Vector3.up * _jumpForce);
            AudioManager.Instance.PlaySFX("Jump");
        }

        //*********************HeadBob*********************
        if (!_isGrounded)
        {
            //Airborne
            HeadBob(idleCounter, 0.03f, 0.03f);
            idleCounter += 0;
            _weaponParent.localPosition = Vector3.Lerp(_weaponParent.localPosition, _TargetWeaponBobPostion, Time.deltaTime * 2f);

            _PlayerSphereCollider.radius = _SlowWalkSoundRaduis;
        }
        else if (HorizontalMovement == 0 && VerticalMovement == 0)
        {
            //idle
            HeadBob(idleCounter, 0.03f, 0.03f);
            idleCounter += Time.deltaTime;
            _weaponParent.localPosition = Vector3.Lerp(_weaponParent.localPosition, _TargetWeaponBobPostion, Time.deltaTime * 2f);

            _PlayerSphereCollider.radius = _SlowWalkSoundRaduis;

            //Remove Gun Accuracy
            _weapon.DisableRunningAccuracy();
        }
        else if (!_CanRun && !_CanSlow)
        {
            //moving
            HeadBob(MovementCounter, 0.085f, 0.085f);
            MovementCounter += Time.deltaTime * 5f;
            _weaponParent.localPosition = Vector3.Lerp(_weaponParent.localPosition, _TargetWeaponBobPostion, Time.deltaTime * 6f);

            _PlayerSphereCollider.radius = _walkSoundRaduis;

            //Give Weapon Inaccuracy
            _weapon.EnableRunningInaccuracy();
        }
        else if (!_CanRun)
        {
            //Slow Walking
            HeadBob(MovementCounter, 0.025f, 0.025f);
            MovementCounter += Time.deltaTime * 2f;
            _weaponParent.localPosition = Vector3.Lerp(_weaponParent.localPosition, _TargetWeaponBobPostion, Time.deltaTime * 6f);

            _PlayerSphereCollider.radius = _SlowWalkSoundRaduis;

        }
        else
        {
            //Sprinting
            HeadBob(MovementCounter, 0.125f, 0.125f);
            MovementCounter += Time.deltaTime * 8f;
            _weaponParent.localPosition = Vector3.Lerp(_weaponParent.localPosition, _TargetWeaponBobPostion, Time.deltaTime * 8f);


            _PlayerSphereCollider.radius = _SprintSoundRaduis;
        }


        //Making the Player able to shoot only when he is not sprinting
        if (_CanRun)
        {
            //Make the unable to shoot while sprinting
            _weapon.PlayerSprinting();
        }
        else
        {
            //Make the able to shoot while sprinting
            _weapon.PlayerNotSprinting();
        }

        //Simulating the landing Sound
        if (_isGrounded && !_isLanded)
        {
            Landed();
            _isLanded = true;
        }
        else if (!_isGrounded)
        {
            _isLanded = false;
        }

        //Hide Crosshair if Axe
        if (_weapon.CurrentGunData.gun == Weapon.Gun.Axe)
        {
            HideCrossHair();
        }

        //Show crosshair if not aiming
        if (Input.GetMouseButtonUp(1) && _weapon.CurrentGunData.gun != Weapon.Gun.Axe)
        {
            ShowCrossHair();
        }

        PlayMovementSoundEffect(_isGrounded, HorizontalMovement, VerticalMovement);

        HandleSprint(_CanRun);
    }

    private void HandleSprint(bool CanRun)
    {
        if(_playerTired) return;

        if (_sprintTimer < 0) 
        {
            _playerTired = true;
            StartCoroutine(WaitTillRested());
            return;
        }

        if (CanRun) _sprintTimer -= Time.deltaTime;
        else if(_sprintTimer < 5f) _sprintTimer += Time.deltaTime;
    }

    IEnumerator WaitTillRested()
    {
        //playing panting sound
        yield return new WaitForSeconds(5f);
        _sprintTimer = 5f;
        _playerTired = false;
    }

    private void PlayMovementSoundEffect(bool isGrounded, float horizontalMovement, float verticalMovement)
    {
        if(isGrounded && (horizontalMovement != 0 || verticalMovement != 0))
        {
            if (_speed == _normalSpeed)
            {
                PlaySoundAsperSpeed(_runStep);
            }
            else if(_speed == _slowWalkSpeed)
            {
                PlaySoundAsperSpeed(_walkStep);
            }
            else if (_speed == _runSpeed)
            {
                PlaySoundAsperSpeed(_sprintStep);
            }
        }
    }

    private void PlaySoundAsperSpeed(float stepSpeed)
    {
        if (_stepTimer >= stepSpeed)
        {
            AudioManager.Instance.PlayMovementSound(1.1f - stepSpeed);
            _stepTimer = 0f;
        }
        else
        {
            _stepTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (_isInCutscene) return;

        //input
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = Input.GetAxisRaw("Vertical");
        bool Run = Input.GetKey(KeyCode.LeftShift);
        bool Jump = Input.GetKeyDown(KeyCode.Space);


        //States
        bool _isGrounded = Physics.Raycast(_groundDetector.position, Vector3.down, 0.5f, _ground);
        bool _CanJump = Jump && _isGrounded;
        bool _CanRun = Run && (VerticalMovement > 0) && !Jump && _isGrounded;


        //Movement
        _Direction = new Vector3(HorizontalMovement, 0, VerticalMovement);
        _Direction.Normalize();

        Vector3 TargetVelocity = transform.TransformDirection(_Direction) * _speed * Time.deltaTime;

        TargetVelocity.y = _playerRig.velocity.y;

        _playerRig.velocity = TargetVelocity;
    }

    #endregion

    #region Private Methods
    void HeadBob(float angle , float xItensity , float yIntensity)
    {
        if (isAiming && _weapon.CurrentGunData.gun != Weapon.Gun.Axe)
        {
            ADSHeadBobSens = 0.01f;
        }
        else
        {
            ADSHeadBobSens = 1f;
        }
        _TargetWeaponBobPostion = _originalWeaponPosition + new Vector3(Mathf.Cos(angle) * xItensity * ADSHeadBobSens, Mathf.Sin(angle * 2) * yIntensity * ADSHeadBobSens, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(other.gameObject != null)
            {
                other.gameObject.GetComponentInParent<EnemyAI>().SensedPlayer();
            }
        }
    }

    private void Landed()
    {
        //Create a Sphere around the origin of sound and capture all the colliders of layer enemy
        Collider[] Enemies = Physics.OverlapSphere(transform.position, _landSoundRaduis, _enemy);

        for(int i =0; i<Enemies.Length; i++) //loop through and alert enemy if caught within the sphere
        {
            EnemyAI ai = Enemies[i].GetComponentInParent<EnemyAI>();
            if(ai) ai.SensedPlayer();
        }


        //Sound SFX
        AudioManager.Instance.PlaySFX("Land");
    }

   private void HideCrossHair() => UIManager.Instance.HideCrossHair();
   private void ShowCrossHair() => UIManager.Instance.ShowCrosshair();

    void Die()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("Death");
        enabled = false;
    }

    void DisablePlayerBeforeCutscene()
    {
        _isInCutscene = true;
    }

    void EnablePlayerAfterCutscene()
    {
        _isInCutscene = false;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= Die;
        EventManager.OnStartCutscene -= DisablePlayerBeforeCutscene;
        EventManager.OnEndCutscene -= EnablePlayerAfterCutscene;
    }
    #endregion

    #region Public Method
    public void PickedUpWeapon(Weapon.Gun Gun)
    {
       switch(Gun)
       {
            case Weapon.Gun.AK47 : _weapon.GotAK();
                UIManager.Instance.ShowGun(Gun);
                break;

            case Weapon.Gun.ShotGun : _weapon.GotShotGun();
                UIManager.Instance.ShowGun(Gun);
                break;
       }
    }

    public void PickedUpWeaponAmmo(Weapon.AmmoType Ammo, Notification notif) => _weapon.GotAmmo(Ammo, notif);

    #endregion
}
