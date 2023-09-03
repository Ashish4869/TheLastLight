using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This scripts deals the Enemy AI functionality
/// 1.Random wandering
/// 2.WayPoint Wandering
/// 3.Chasing the player
/// 4.Losing the player
/// 5.Attacking
/// </summary>

#region ENUMS
public enum EnemyState
{
    Wandering,
    Chasing,
    Searching,
    Attacking
};
#endregion

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(RagdollDeath))]
[RequireComponent(typeof(EnemySoundManager))]
public class EnemyAI : MonoBehaviour
{
    #region Variables
    private Transform _Target;
    private NavMeshAgent _agent;
    private EnemyState _enemyState;
    private Animator _ZombieAnimator;
    private EnemySoundManager _enemySoundManager;

    public float _Wanderspeed;
    public float _ChaseSpeed;
    public bool _isWanderEnemy;
    public Transform[] _wayPoints;
    public bool _isBoss;

    private float _attackDistance = 2f;
    private float _fov = 120f;
    private float _visibilityDistanceThreshold = 20f;
    private Vector3 _wanderPoint;
    private float _wanderRaduis = 10f;
    private int _wayPointIndex = 0;
    private float _FindingEnemyTimer;
    private float _SearchingTime = 5f;
    private bool _playerDead;
    private bool _canAttackAgain = true;
    private bool _isInCutscene = false;

    
    #endregion

    #region MonoBehaviourCallBacks
    private void Awake()
    {
        _agent = transform.parent.GetChild(0).GetComponentInParent<NavMeshAgent>();
        _Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _ZombieAnimator = GetComponent<Animator>();
        _enemySoundManager = GetComponent<EnemySoundManager>();
        EventManager.OnPlayerEnterExitCar += ChangeTarget;
        EventManager.OnStartCutscene += DisableEnemy;
        EventManager.OnEndCutscene += EnableEnemy;

        if(_isBoss)
        {
            _fov = 360;
            _visibilityDistanceThreshold = 1000f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _enemyState = EnemyState.Wandering;
        _agent.speed = _Wanderspeed;
        _wanderPoint = RandomWanderPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInCutscene) return;
        if (_isBoss) return;

        //try to detect the player only if he is undetected by this enemy
        if(_enemyState == EnemyState.Wandering)
        {
            DetectPlayer();

            if(_isWanderEnemy)
            {
                Wander();
            }
            else
            {
                WayPointWander();
            }

            _enemySoundManager.StateChanged(_enemyState);

        }

        //if we have detected the enemy
        if (_enemyState == EnemyState.Chasing)
        {
            ChasePlayer();
        }

        //search player
        if(_enemyState == EnemyState.Searching)
        {
            SearchPlayer();
        }

        if(_enemyState == EnemyState.Attacking)
        {
            Attack();
        }
    }



    private void OnDestroy()
    {
        EventManager.OnPlayerEnterExitCar -= ChangeTarget;
        EventManager.OnStartCutscene -= DisableEnemy;
        EventManager.OnEndCutscene -= EnableEnemy;

    }

    //Drawing Gizmos for better visualizations and debugging
    private void OnDrawGizmos()
    {
        if(!_isWanderEnemy)
        {
            for(int i = 0; i < _wayPoints.Length-1; i++)
            {
                Gizmos.DrawLine(_wayPoints[i].position, _wayPoints[i + 1].position);
            }

            Gizmos.DrawLine(_wayPoints[_wayPoints.Length - 1].position, _wayPoints[0].position);
        }

        Gizmos.DrawSphere(_wanderPoint, 1);

    }
    #endregion

    #region Private Functions
    void Wander()
    {
        if(_wanderPoint.x == Mathf.Infinity) //sometimes we get infinity wander points idk why
        {
            _wanderPoint = RandomWanderPoint();
        }

        if(Vector3.Distance(transform.position , _wanderPoint) < 1f) //The enemy is close enough to the wander point generate new one or proceed to moving towards it
        {
            _wanderPoint = RandomWanderPoint();
            _agent.SetDestination(_wanderPoint);
        }
        else
        {
            _agent.SetDestination(_wanderPoint);
        }
    }

    Vector3 RandomWanderPoint()
    {
        Vector3 randomWanderPoint = (Random.insideUnitSphere * _wanderRaduis) + transform.position; //generating a random sphere from the enemy as the randomm point

        NavMeshHit navhit;
        NavMesh.SamplePosition(randomWanderPoint, out navhit, _wanderRaduis, -1); //geting a point within the confines of the navmesh
        return new Vector3(navhit.position.x, transform.position.y, navhit.position.z);
    }


    void WayPointWander()
    {
        if(Vector3.Distance(transform.position , _wayPoints[_wayPointIndex].position) < 3f) //go to next way point if close enough , else go going towards waypoint
        {
            _wayPointIndex = (_wayPointIndex + 1) % (_wayPoints.Length);
            _agent.SetDestination(_wayPoints[_wayPointIndex].position);
        }
        else
        {
            _agent.SetDestination(_wayPoints[_wayPointIndex].position);
        }
    }

    void DetectPlayer()
    {
        if (_playerDead) return;

        //making sure that the player is close enough to the enemy to be detected
        if (Vector3.Distance(transform.position, _Target.position) < _visibilityDistanceThreshold)
        {
            //chases the player if the angle from the forward vector of the enemy and player position (from the enemies view which local space) is less than half the fov
            if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(_Target.position)) < _fov / 2)
            {
                RaycastHit hit;

                //prevents player from being detected through walls
                if(Physics.Linecast(transform.position , _Target.position, out hit , -1))
                {
                    if(hit.transform.CompareTag("Player")) //making sure that we hit the player
                    {
                        _enemyState = EnemyState.Chasing;
                    }
                }
               
            }
        }
        else if (_enemyState == EnemyState.Chasing) //if enemy loses player in a chase
        {
            _enemyState = EnemyState.Searching;
        }

    }

    public void ChasePlayer()
    {
        _agent.speed = _ChaseSpeed;
        _agent.isStopped = false;
        _agent.SetDestination(_Target.position);
        _ZombieAnimator.SetBool("Chasing", true);

        DetectPlayer();
        _enemySoundManager.StateChanged(_enemyState);
        //if we get close enough for attack
        if (Vector3.Distance(transform.position, _Target.position) < _attackDistance) _enemyState = EnemyState.Attacking;
    }

    public void Attack()
    {
        _ZombieAnimator.SetBool("Attacking", true);
        _agent.speed = 0;
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;

        //if we exceed enemies attack distance
        if (Vector3.Distance(_Target.position, transform.position) > _attackDistance)
        {
            _enemyState = EnemyState.Chasing;
            _ZombieAnimator.SetBool("Attacking", false);
        }
    }

    void SearchPlayer()
    {
        _agent.SetDestination(_Target.position); //keep following the player
        _FindingEnemyTimer += Time.deltaTime;   // incremenet the timer

        if (_FindingEnemyTimer > _SearchingTime) //Makes the enemy search for a particular time and then gives up
        {
            _FindingEnemyTimer = 0f;
            _agent.speed = _Wanderspeed;
            _enemyState = EnemyState.Wandering;
            _ZombieAnimator.SetBool("Chasing", false);
        }
    }

    void ChangeTarget()
    {
        _Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void DisableEnemy()
    {
        _isInCutscene = true;
    }

    void EnableEnemy()
    {
        _isInCutscene = false;
    }

   
    #endregion

    #region Public Functions
    public void SensedPlayer()
    {
        _enemyState = EnemyState.Chasing; //chase the player if sound is made within the range of perception of sound or 6th sense
        
    }

    public void SetChaseSpeed(float speed)
    {
        _ChaseSpeed = speed;
    }
    #endregion
}
