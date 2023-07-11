using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Implements Singleton Pattern
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    float _playerHealth = 100; //we know that the player starts with 100 health

    [SerializeField] GameObject[] _crateSpawnItem; //we store elementes that can be picked up
    [SerializeField] GameObject _player;
    
    [Header("Blood Stuff")] //once done, try putting it in another script
    [SerializeField] GameObject _bloodSplash;
    [SerializeField] GameObject[] _bloodSpot;

    bool _isGamePaused = false;
    bool _isInDialouge = false;

    [SerializeField] PlayableDirector _cutscene;


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

       
        
    }
    #endregion

    #region MonoBehviour CallBacks

    Vector3 point = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point, 0.25f);
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
        point = EnemyBodyPart.point;
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
        _player.SetActive(false);
        _cutscene.Play();
    }
    #endregion

    #region Private Functions
    #endregion
}
