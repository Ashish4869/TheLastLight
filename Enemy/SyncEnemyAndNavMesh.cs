using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This syncs the rigidbody position of the player with the transform updated by navmesh, this will result in blooddecal being detected and placed appropritaley
/// </summary>

public class SyncEnemyAndNavMesh : MonoBehaviour
{
    Rigidbody _enemyRoot;
    private void Awake()
    {
        _enemyRoot = transform.parent.GetChild(1).GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _enemyRoot.position = transform.position;
        _enemyRoot.rotation = transform.rotation;
    }
}
