using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the Ui for main Menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region Variables
    [Header("UI GameObjects")]
    [SerializeField] GameObject _anyKeyPressed;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _continue;
    [SerializeField] GameObject _settingsMenu;

    [Header("Animators")]
    [SerializeField] Animator _settingsOverlayAnimator;

    bool _anyKeyPressedBool = false;
    #endregion

    #region MonoBehavioursCallBacks

    private void Start()
    {
        TextMeshProUGUI _continueText = _continue.GetComponent<TextMeshProUGUI>();
        Button _continueButton = _continue.GetComponent<Button>();

        if(GameManager.Instance.HasValueFromDisk())
        {
            _continueButton.interactable = true;
            _continueText.color = new Color(1f, 1f, 1f, 1);
        }
        else
        {
            _continueButton.interactable = false;
            _continueText.color = new Color(1f, 1f, 1f, 0.5f);
           
        }
    }

   

    private void Update()
    {
        if(Input.anyKey && _anyKeyPressedBool == false)
        {
            _anyKeyPressedBool = true;
            _anyKeyPressed.SetActive(false);
            _mainMenu.SetActive(true);
        }
    }
    #endregion

    #region Private Methods
   
    #endregion

    #region Public Functions
    public void StartNewGame()
    {
        string path = Application.persistentDataPath + "/gameData.data";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //start the new game
        FindAnyObjectByType<LevelLoader>().LoadNextLevel();
    }

    public void ContinueGame()
    {
        int currentLevel = SaveData.Instance.GetCurrentLevel();

        //Load proper Level
        FindAnyObjectByType<LevelLoader>().LoadParticularLevel(currentLevel);
    }

    public void ShowSettings()
    {
        _settingsOverlayAnimator.SetTrigger("FadeIn");
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
    }

    public void HideSettings()
    {
        _settingsOverlayAnimator.SetTrigger("FadeOut");
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadTestlevel(int level)
    {
        FindAnyObjectByType<LevelLoader>().LoadParticularLevel(level);
    }
    #endregion




}
