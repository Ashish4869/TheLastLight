using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Transitions from animation to ragdoll death
/// <
/// /summary>

[RequireComponent(typeof(Rigidbody))]
public class RagdollDeath : MonoBehaviour
{
    #region Variables
    private EnemyAI _enemiesAI;
    NavMeshAgent _enemyAgent;
    private Animator _enemyAnimator;

    Rigidbody parentRigid;

    Rigidbody[] _rigidbodies;
    CharacterJoint[] _joints;
    Collider[] _colliders;
    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _joints = GetComponentsInChildren<CharacterJoint>();
        _colliders = GetComponentsInChildren<Collider>();
        parentRigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _enemyAgent = transform.parent.GetChild(0).GetComponent<NavMeshAgent>();
        _enemiesAI = GetComponent<EnemyAI>();
        _enemyAnimator = GetComponent<Animator>();
        DisableRagdoll();
    }
    #endregion

    #region Private Functions
    void DisableRagdoll()
    {
        foreach (CharacterJoint joint in _joints) joint.enableCollision = false;

        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        parentRigid.useGravity = false;
        parentRigid.isKinematic = true;
    }

    void EnableRagDoll()
    {
        foreach (CharacterJoint joint in _joints) joint.enableCollision = true;
        
        foreach(Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }

        parentRigid.velocity = Vector3.zero;
        parentRigid.detectCollisions = true;
        parentRigid.useGravity = true;
        parentRigid.isKinematic = false;
       
    }
    #endregion

    #region Public Functions
    public void EnableRagDollEffect(Vector3 FallDeath, Rigidbody BodyPart)
    {
        //Disable componenets that can hinder ragdoll functioning
        _enemyAnimator.enabled = false;
        _enemiesAI.enabled = false;
        _enemyAgent.enabled = false;

        EnableRagDoll();

        BodyPart.AddForce(FallDeath * 200, ForceMode.Impulse);
 
    }

    public void DisableAllRigidBodies()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }
    #endregion
}
