using UnityEngine;

/// <summary>
/// Object Pool for bullet holes
/// </summary>

public class BulletHolePool : MonoBehaviour
{
    public static BulletHolePool _instance;
    public GameObject _bulletHolePrefab;
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
            _pool[i] = Instantiate(_bulletHolePrefab, transform);
            _pool[i].SetActive(false);
        }
    }

    public static void Take(Vector3 HitPoint, Vector3 HitNormal)
    {
        if(++_instance._currentPoolIndex >= _instance._pool.Length)
        {
            _instance._currentPoolIndex = 0;
        }

        _instance._pool[_instance._currentPoolIndex].SetActive(false);
        _instance._pool[_instance._currentPoolIndex].transform.position = HitPoint + HitNormal * 0.001f;
        _instance._pool[_instance._currentPoolIndex].transform.LookAt(HitPoint + HitNormal);
        _instance._pool[_instance._currentPoolIndex].SetActive(true);
    }

}
