using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float _speed = 10f;
    bool _isBossDead = false;

    private void Start()
    {
        EventManager.OnBossDefeated += LandHelicopter;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBossDead) return;
         transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }

    public void LandHelicopter()
    {
        _isBossDead = true;
    }

    private void OnDisable()
    {
        EventManager.OnBossDefeated += LandHelicopter;
    }
}
