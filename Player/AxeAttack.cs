using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deals with the attack point when the axe is swayed
/// </summary>
public class AxeAttack : MonoBehaviour
{
    public float _attackRaduis;
    public LayerMask _enemyMask,_destructble;
    public float _axeDamage = 25f;

    private void Update()
    {
        EnemyAxeHit();
        DestructbleAxeHit();
    }

    void EnemyAxeHit()
    {
        Collider[] Enemyhit = Physics.OverlapSphere(transform.position, _attackRaduis, _enemyMask); //storing all the colliders of layer enemy at distance less than 2 units from the axe's edge

        if (Enemyhit.Length > 0) //if we hit something with layer enemy
        {
            Enemyhit[0].gameObject.GetComponentInParent<HealthManager>().TakeDamage(_axeDamage, -Enemyhit[0].gameObject.transform.forward, Enemyhit[0].attachedRigidbody);
            GameManager.Instance.ShowBloodParticleEffect(Enemyhit[0]);
            AudioManager.Instance.PlaySFX("AxeHit");
            gameObject.SetActive(false); //to prevent multiple hit detecting in a single blow
        }
    }

    void DestructbleAxeHit()
    {
        Collider[] BoxHit = Physics.OverlapSphere(transform.position, _attackRaduis, _destructble); //storing all the colliders of layer enemy at distance less than 2 units from the axe's edge

        if (BoxHit.Length > 0) //if we hit something with layer enemy
        {
            BoxHit[0].gameObject.GetComponent<Destructible>().DestroyObject();
            gameObject.SetActive(false); //to prevent multiple hit detecting in a single blow
        }
    }
}
