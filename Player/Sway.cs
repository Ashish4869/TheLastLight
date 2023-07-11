using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    #region Variables
    float _intensity = 5f;
    float _smoothness = 5f;

    Quaternion _OriginRotation;
    #endregion

    #region MonoBehavivourCallbacks
    
    // Start is called before the first frame update
    void Start()
    {
        _OriginRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSway();
    }
    #endregion

    #region Private Variables
    void UpdateSway()
    {
        float Hmove = Input.GetAxisRaw("Mouse X");
        float Vmove = Input.GetAxisRaw("Mouse Y");

        Quaternion xAdj = Quaternion.AngleAxis(-_intensity * Hmove, Vector3.up);
        Quaternion yAdj = Quaternion.AngleAxis(_intensity * Vmove, Vector3.right);

        Quaternion TargetRotation = _OriginRotation * xAdj * yAdj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, TargetRotation , Time.deltaTime * _smoothness);
    }
    #endregion
}
