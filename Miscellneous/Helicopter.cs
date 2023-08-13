using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the helicopter from whereever it is to the landing zone
/// </summary>
public class Helicopter : MonoBehaviour
{
    public Transform[] _wayPoints;
    public float _speed = 0.0000000001f;
    bool _isBossDeath = false;
    int _wayPointIndex = 0;

    private void Start()
    {
        EventManager.OnBossDefeated += BossDefeated;
    }
    private void Update()
    {
        if (!_isBossDeath) return;

        if (_wayPointIndex == _wayPoints.Length) return; //if we exhausted the array, return

        transform.LookAt(_wayPoints[0].position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); ;



        if (Vector3.Distance(transform.position,_wayPoints[_wayPointIndex].position) > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_wayPointIndex].position, _speed * Time.deltaTime);
        }
        else
        {
            _wayPointIndex++;
        }
        
    }

    void BossDefeated()
    {
        _isBossDeath = true;
    }

    private void OnDisable()
    {
        EventManager.OnBossDefeated -= BossDefeated;
    }
}
