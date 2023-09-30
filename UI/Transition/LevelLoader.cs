using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator _transition;
    public float _transitionTime = 1;
    float _waitTime = 3;
    public GameObject _loader;
    AsyncOperation _loadingOperation;

 
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(TriggerLoader());
    }

    public void LoadParticularLevel(int level)
    {
        StartCoroutine(LoadLevel(level));
        StartCoroutine(TriggerLoader());
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(_waitTime);
        _loadingOperation = SceneManager.LoadSceneAsync(levelIndex);
    }

    IEnumerator TriggerLoader()
    {
        yield return new WaitForSeconds(_transitionTime);
        _loader.SetActive(true);
    }

}
