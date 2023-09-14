using UnityEngine;

public class DialougeObjects : Interactable
{
    [SerializeField] Dialouge[] _dialouges;
    int _currentDialouge = 0;
    [SerializeField] bool _isObjectiveInteractable = false;
    bool _thisDialouge = false; //to prevent scripts of the same type triggering the dialouge
    [SerializeField] bool _disposable = false;

    protected override void Interact()
    {
        base.Interact();
        _currentDialouge = 0;
        UIManager.Instance.SetUpDialougeUI();
        GameManager.Instance.SetGamePauseStatus(true);
        GameManager.Instance.SetUpDialougeSystem();
        UIManager.Instance.SetDialouge(_dialouges[_currentDialouge]._speaker, _dialouges[_currentDialouge]._dialouge);
        _currentDialouge++;
        _thisDialouge = true;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.DialougeStatus() && _thisDialouge)
        {
            if (Input.GetMouseButtonDown(0) && !UIManager.Instance.IsDialougeAnimating())
            {
                //if we have more dialouges left
                if (_currentDialouge < _dialouges.Length)
                {
                    UIManager.Instance.SetDialouge(_dialouges[_currentDialouge]._speaker, _dialouges[_currentDialouge]._dialouge);
                    _currentDialouge++;
                }
                else
                {
                    UIManager.Instance.DisableDialougeUI();
                    GameManager.Instance.SetGamePauseStatus(false);
                    GameManager.Instance.DisableDialougeSystem();
                    _thisDialouge = false;

                   
                    if (_isObjectiveInteractable) GetComponent<ObjectiveInteractables>().RunObjective();
                    if (_disposable)
                    {
                        gameObject.SetActive(false);
                        FindAnyObjectByType<DispoableItemManager>().UpdateDisposableStatus();
                    }
                }
            }
        }
    }
}
