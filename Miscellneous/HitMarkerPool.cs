using UnityEngine;

/// <summary>
/// Object Pool for bullet holes
/// </summary>

public class HitMarkerPool : MonoBehaviour
{
    public static HitMarkerPool _instance;
    public GameObject _hitMarkerPrefab;
    public int _poolSize;
    GameObject[] _pool;
    int _currentPoolIndex = 0;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }


        _pool = new GameObject[_poolSize];

        for(int i = 0; i < _poolSize; i++)
        {
            _pool[i] = Instantiate(_hitMarkerPrefab, transform);
            _pool[i].SetActive(false);
        }
    }

    public static void Take(Vector3 Position, Quaternion Rotation)
    {
        if(++_instance._currentPoolIndex >= _instance._pool.Length)
        {
            _instance._currentPoolIndex = 0;
        }

        _instance._pool[_instance._currentPoolIndex].SetActive(false);
        _instance._pool[_instance._currentPoolIndex].transform.position = Position;
        _instance._pool[_instance._currentPoolIndex].transform.rotation = Rotation;
        _instance._pool[_instance._currentPoolIndex].SetActive(true);
    }

}
