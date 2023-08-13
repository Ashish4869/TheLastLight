using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnragedRun : StateMachineBehaviour
{
    Transform _player;
    EnemyAI _enemyAI;
    Transform _bossTransform;
    float _attackRange = 2f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemyAI = animator.GetComponent<EnemyAI>();
        _bossTransform = animator.GetComponent<Transform>();
        _enemyAI.SetChaseSpeed(10f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyAI.ChasePlayer();

        if (Vector3.Distance(_player.position, _bossTransform.transform.position) < _attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        _enemyAI.Attack();
    }

    
}
