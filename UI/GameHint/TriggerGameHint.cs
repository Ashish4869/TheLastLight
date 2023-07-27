using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TriggerGameHint : MonoBehaviour
{
    [SerializeField] [TextArea(3,5)] string _hintText;
    [SerializeField] GameObject _gameHint;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) //if the player has collided with this trigger
        {
            _gameHint.GetComponent<Animator>().SetTrigger("ShowHint");
            _gameHint.GetComponentInChildren<TextMeshProUGUI>().text = _hintText;
            Destroy(gameObject);
        }
    }
}
