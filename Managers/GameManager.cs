using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Implements Singleton Pattern
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    float _playerHealth = 100; //we know that the player starts with 100 health

    [SerializeField] GameObject[] _crateSpawnItem; //we store elementes that can be picked up
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _levelLoader;
    
    [Header("Blood Stuff")] //once done, try putting it in another script
    [SerializeField] GameObject _bloodSplash;
    [SerializeField] GameObject[] _bloodSpot;

    bool _isGamePaused = false;
    bool _isInDialouge = false;
    bool _isInCar = false;

    int _currentLevel = 1;


    [SerializeField] GameObject _cutSceneCam;


    #endregion

    #region Singleton Implementation

    public static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null) _instance = new GameObject().AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        if (_currentLevel == 2)
        {
            _isInCar = true;
        }
    }
    #endregion

    #region MonoBehviour CallBacks

    private void Start()
    {
        PlayCutscene(); //play this in start, as other scripts need time to register for the event before the event is played.
    }
    #endregion

    #region Public Functions
    public void SetPlayerHealth(float health)
    {
        _playerHealth = health;
    }

    public float GetPlayerHealth() => _playerHealth;

    public void ShowDamageOverlay() => UIManager.Instance.ShowDamageOverlay();

    public void PickedUpHealthPack() => UIManager.Instance.PickedUpMedPack();

    public void SpawnRandomCrateItem(Transform Position)
    {
        int itemProb = Random.Range(0, 100);
        if(itemProb > 50)
        {
            Instantiate(_crateSpawnItem[Random.Range(0, _crateSpawnItem.Length)], Position.position , Quaternion.identity);
        }
    }

    public void ShowBloodParticleEffect(RaycastHit EnemyBodyPart)
    {
        ImplantBloodDecal(EnemyBodyPart);
        GameObject bloodSplash = Instantiate(_bloodSplash, EnemyBodyPart.transform);
        bloodSplash.transform.position = EnemyBodyPart.point + EnemyBodyPart.normal * 0.1f;
        bloodSplash.transform.LookAt(EnemyBodyPart.point + EnemyBodyPart.normal);
    }

    public void ShowBloodParticleEffect(Collider EnemyBodyPart)
    {
        Instantiate(_bloodSplash, EnemyBodyPart.transform);
    }

    public void ImplantBloodDecal(RaycastHit EnemyBodyPart)
    {
        GameObject BloodSpot = _bloodSpot[Random.Range(0, _bloodSpot.Length)]; 
        GameObject bloodSplash = Instantiate(BloodSpot, EnemyBodyPart.transform);
        bloodSplash.transform.position = EnemyBodyPart.point + EnemyBodyPart.normal * 0.01f;
        bloodSplash.transform.LookAt(EnemyBodyPart.point + EnemyBodyPart.normal);
        bloodSplash.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
    }

    public void SetUpDialougeSystem()
    {
        Time.timeScale = 0f;
        _isInDialouge = true;
    }

    public void DisableDialougeSystem()
    {
        Time.timeScale = 1f;
        _isInDialouge = false;
    }

    public void SetGamePauseStatus(bool condition) => _isGamePaused = condition;
    public bool GamePaused() => _isGamePaused;

    public void SetDialoguePauseStatus(bool condition) => _isInDialouge = condition;
    public bool DialougeStatus() => _isInDialouge;

    public void PlayCutscene()
    {
        FindAnyObjectByType<EventManager>().OnStartCutsceneEvent();
        FindAnyObjectByType<CutSceneManager>().PlayCutscene();
        _cutSceneCam.SetActive(true);
        _levelLoader.SetActive(false);
    }

    //Car Mechanics
    public void HidePlayer()
    {
        _isInCar = true;
        _player.SetActive(false);
    }

    public void TransitionToPlayer()
    {
        _isInCar = false;
        _player.SetActive(true);
    }

    public void PlacePlayerNearCar(Vector3 CarPosition)
    {
        _player.transform.position = CarPosition + Vector3.left * 2 + Vector3.up;
    }

    public bool IsPlayerInCar() => _isInCar;

    //Cutscene related
    public void CutSceneFinished()
    {
        FindAnyObjectByType<EventManager>().OnEndCutsceneEvent();
        _levelLoader.SetActive(true);
    }

    public void LoadlevelAfterCutscene()
    {
        _levelLoader.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Game Data
    public int GetCurrentLevel() => _currentLevel;

    
    #endregion

    #region Private Functions
    #endregion
}
