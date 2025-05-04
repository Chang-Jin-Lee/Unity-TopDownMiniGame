using UnityEngine;

public class CameraController : MonoBehaviour
{
   //---------------------------------------------
    public Transform _cameraTransform;
    public Transform _targetTransform;
    public Vector3 targetOffset = new Vector3(0, 1, 0);    // Player Offset

    public float fDistance = 15.0f;          //Editor Dynamic Distance
    public float fPitch = 30.0f;
    public float fYaw = 0.0f;
    
    private float fVelocity = 0.0f;
    float fDistance_cur;
    float fSmoothTime = 0.1F;   // smooth, smaller is faster.

    // about move
    Vector3 clickTargetPos = new Vector3(0,0,0);

    //---------------------------------------------
    void Awake()
    {
        if (!_cameraTransform && Camera.main)
            _cameraTransform = Camera.main.transform;
        if (!_cameraTransform)
        {
            Debug.Log("Please assign a camera to the CameraController script.");
            enabled = false;
        }

        if (!_targetTransform)
        {
            Debug.Log("Please assign a target Transform.");
        }

    }

    void Start()
    {
        clickTargetPos = _targetTransform.transform.position;
    }

    void LateUpdate()
    {
        if (!_targetTransform) return;
        Quaternion currentRotation = Quaternion.Euler(fPitch, fYaw, 0);

        // target position
        Vector3 targetPos = _targetTransform.position + targetOffset;

        // camera position
        fDistance_cur = Mathf.SmoothDamp(fDistance_cur, fDistance, ref fVelocity, fSmoothTime);
        _cameraTransform.position = targetPos + currentRotation * Vector3.back * fDistance_cur;

        // camera rotation
        Vector3 relativePos = targetPos - _cameraTransform.position;
        _cameraTransform.rotation = Quaternion.LookRotation(relativePos); // * Quaternion.Euler (fPitch, fYaw, 0 );	
    }
}
