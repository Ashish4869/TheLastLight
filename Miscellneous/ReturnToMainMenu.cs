using UnityEngine.SceneManagement;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    public void ReturnToMainMenuFunc()  
    {
        SceneManager.LoadScene(0);
    }
}
