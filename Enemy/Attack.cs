using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deals Damage to the player when attacking
public class Attack : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform _rightHand;
    [SerializeField] Transform _leftHand;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] float _zombieDamage = 5f;
    [SerializeField] float _attackRadius = 2f;

    #endregion
    #region MonoBehaviour Callbacks
   
    #endregion

    #region Public Methods
    void DealDamageLeftHand()
    {
        DealDamageToPlayer(_leftHand.position);
    }

    void DealDamageRightHand()
    {
        DealDamageToPlayer(_rightHand.position);
    }

    void DealDamageToPlayer(Vector3 HandTransform)
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, _attackRadius, _playerMask); //storing all the colliders of layer enemy at distance less than 2 units from the axe's edge
        
        foreach(Collider collider in hit)
        {
            if(collider.GetType() == typeof(CapsuleCollider)) //we hard coding for now
            {
                collider.GetComponent<HealthManager>().TakeDamage(_zombieDamage);
            }
        }
    }
    #endregion

    #region Private Methods
    #endregion

}
