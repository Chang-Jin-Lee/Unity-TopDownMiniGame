using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   //---------------------------------------------
    public Transform _cameraTransform;
    public Transform _targetTransform;

    public Vector3 targetOffset = new Vector3(0, 1, 0);    //��ġ

    public float fDistance = 15.0f;          //Editor �켱 ����
    public float fPitch = 30.0f;
    public float fYaw = 0.0f;
    
    private float fVelocity = 0.0f;
    float fDistance_cur;
    float fSmoothTime = 0.1F;   // smooth, smaller is faster.

    // about move
    public float movespeed = 50.0f;
    float h_move = 0;
    float v_move = 0;
    Vector3 movement;
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
