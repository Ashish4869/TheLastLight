using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is for disabling stuff so that we roam freely
/// </summary>
public class TrailerManager : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject UI;


    private void Start()
    {
        weapon.SetActive(false);
        UI.SetActive(false);
    }

    private void Update()
    {   

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindAnyObjectByType<LevelLoader>().LoadParticularLevel(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindAnyObjectByType<LevelLoader>().LoadParticularLevel(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindAnyObjectByType<LevelLoader>().LoadParticularLevel(7);
        }
    }

}
