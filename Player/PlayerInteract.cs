using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Camera _cam;
    float _distance = 2f;
    [SerializeField] LayerMask _interactable;
    #endregion

    #region MonoBehaviour Callbacks

    void Update()
    {
        UIManager.Instance.ClearButtonPrompt();

        RaycastHit hitinfo;
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);
        Debug.DrawLine(ray.origin, ray.direction * _distance);

        if(Physics.Raycast(ray,out hitinfo,_distance, _interactable))
        {
            Interactable InteractableObject = hitinfo.collider.gameObject.GetComponent<Interactable>();
            if (InteractableObject != null)
            {
                if(!GameManager.Instance.DialougeStatus()) UIManager.Instance.SetButtonPrompt(InteractableObject._promptMessage);
                else UIManager.Instance.ClearButtonPrompt();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractableObject.BaseInteract();
                }
            }
        }
    }

    #endregion
}
