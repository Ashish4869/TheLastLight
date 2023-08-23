using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Camera _cam;
    float _distance = 2f;
    [SerializeField] LayerMask _interactable;

    bool _isInCutscene = false;
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        EventManager.OnStartCutscene += EnterCutscene;
        EventManager.OnEndCutscene += ExitCutscene;
    }

    void Update()
    {
        UIManager.Instance.ClearButtonPrompt();

        RaycastHit hitinfo;
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);

        if(Physics.Raycast(ray,out hitinfo,_distance, _interactable))
        {
            Interactable InteractableObject = hitinfo.collider.gameObject.GetComponent<Interactable>();
            if (InteractableObject != null)
            {
                if (!GameManager.Instance.DialougeStatus())
                {
                    if (!_isInCutscene)
                    { UIManager.Instance.SetButtonPrompt(InteractableObject._promptMessage); }
                    else UIManager.Instance.ClearButtonPrompt();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractableObject.BaseInteract();
                }
            }
        }
    }


    private void OnDisable()
    {
        EventManager.OnStartCutscene -= EnterCutscene;
        EventManager.OnEndCutscene -= ExitCutscene;
    }
    #endregion

    #region Private Methods
    private void EnterCutscene()
    {
        _isInCutscene = true;
    }

    private void ExitCutscene()
    {
        _isInCutscene = false;
    }


    #endregion
}
