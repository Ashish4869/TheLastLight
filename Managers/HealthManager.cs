using System.Collections;
using UnityEngine;

/// <summary>
/// This manages the health of the all the Enemies and the player in the scene
/// </summary>

public class HealthManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private bool _isEnemy;
    bool _isZombieDead = false;

    [SerializeField]
    private bool _isBoss;

    EventManager _eventManager;
    private bool _isPlayerDead = false; 

    [SerializeField] float _health = 100f;

    RagdollDeath _ragDollDeath;
    EnemyManager _enemyManager;

    #endregion

    #region MonoBehaviour CallBacks
    private void Awake()
    {
        _ragDollDeath = GetComponent<RagdollDeath>();
        _eventManager = FindObjectOfType<EventManager>();
        _enemyManager = GetComponentInParent<EnemyManager>();
    }

    
    #endregion

    #region Private Methods
    void Death(Vector3 Falldeath, Rigidbody BodyPart)
    {
        //if we kill the enemy
        if(_isEnemy) // prevent the enemy from getting shot again having the force played mulitple times
        {
            GetComponent<EnemySoundManager>().DeathSound();
           _ragDollDeath.EnableRagDollEffect(Falldeath,BodyPart);
            StartCoroutine(FadeOutZombie());
        }
    }

    IEnumerator FadeOutZombie()
    {
        yield return new WaitForSeconds(2f);

        _ragDollDeath.DisableAllRigidBodies();

        float time = 0;

        while(time < 1) // let the zombie sink through the ground
        {
            transform.position += Vector3.down * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
        _enemyManager.UpdateZombieStatus();
    }

    

    
    #endregion


    #region Public Methods
    public void TakeDamage(float damage , Vector3 Falldirection, Rigidbody BodyPart)
    {
        if (_isZombieDead) return;
        _health -= damage;

        if(_isBoss)
        {
            if(_health <= 500)
            {
                GetComponent<Animator>().SetBool("IsEnraged", true);
            }
        }

        if (_health <= 0)
        {
            _isZombieDead = true;

            if (_isBoss)
            {
                GetComponent<Animator>().SetTrigger("Death");
                GetComponentInParent<BossHandler>().HandlePostBossDeath();
                return;
            }
            else
            {
                Death(Falldirection, BodyPart);
            }
            
           

        }
    }

    public void TakeDamage(float damage)
    {
        if (_isPlayerDead) return;

        if (_health < 0)
        {
            _isPlayerDead = true;
            _eventManager.OnPlayerDeathEvent(); //calls event for player death
            AudioManager.Instance.StopPlayingAudio("HeartPounding");
            AudioManager.Instance.StopPlayingAudio("HeavyBreathing");
            AudioManager.Instance.PlaySFX("Death");
            return;
        }


        _health -= damage;
        AudioManager.Instance.PlaySFX("Damage" + Random.Range(1, 3).ToString());
        GameManager.Instance.ShowDamageOverlay();
        GameManager.Instance.SetPlayerHealth(_health);
    }

    public bool IsDead() => _isZombieDead;
    #endregion
    

}
