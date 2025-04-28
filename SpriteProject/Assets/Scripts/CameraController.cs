using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   //---------------------------------------------
    public Transform _cameraTransform;
    public Transform _targetTransform;
    public Rigidbody _targetrb;

    public Vector3 targetOffset = new Vector3(0, 1, 0);    //위치

    public float fDistance = 15.0f;          //Editor 우선 참조
    public float fPitch = 30.0f;
    public float fYaw = 0.0f;

    //float minimumX = -360F;
    //float maximumX = 360F;
    //float minimumY = -0F;   // fPitch max
    //float maximumY = 90F;
    //float minimumZ = 1F;    // fDistance max
    //float maximumZ = 30F;

    private float fVelocity = 0.0f;
    float fDistance_cur;
    float fSmoothTime = 0.1F;   // smooth, smaller is faster.

    //private float angleVelocity = 0.0f;
    //private float angularSmoothTime = 0.2f;
    //private float angularMaxSpeed = 15.0f;

    // about move
    public float movespeed = 50.0f;
    float h_move = 0;
    float v_move = 0;
    Vector3 movement;
    Vector3 clickTargetPos = new Vector3(0,0,0);

    //---------------------------------------------
    //enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    //RotationAxes axes = RotationAxes.MouseXAndY;
    //float sensitivityX = 15F;
    //float sensitivityY = 15F;
    //float sensitivityZ = 1.5F;

    //float rotationY = 0F;

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

        //_targetTransform = transform;		

        if (!_targetTransform)
        {
            Debug.Log("Please assign a target Transform.");
        }

    }

    void Start()
    {
        clickTargetPos = _targetTransform.transform.position;
    }

    void Update()
    {

        //if (Input.GetMouseButton(1)) //우클릭으로 카메라 조정
        //{
        //    fYaw += Input.GetAxis("Mouse X");
        //    //fYaw = Mathf.Clamp (fYaw, minimumX, maximumX);			
        //    fPitch -= Input.GetAxis("Mouse Y");
        //    fPitch = Mathf.Clamp(fPitch, minimumY, maximumY);
        //}

        //fDistance -= Input.GetAxis("Mouse ScrollWheel");
        //fDistance = Mathf.Clamp(fDistance, minimumZ, maximumZ);


        //Move1();
        //Move2();
    }

    void Move1()
    {
        h_move = Input.GetAxisRaw("Horizontal");   // -1, 0, 1
        v_move = Input.GetAxisRaw("Vertical");

        movement.Set(h_move, 0, v_move);    // x,z 이동
        Quaternion q = Quaternion.LookRotation(movement.normalized); q.x = 0; q.z = 0;  // y축 고정 
        _targetTransform.transform.localRotation = q;

        //_targetTransform.transform.LookAt(movement);
        movement = movement.normalized * movespeed * Time.deltaTime;
        _targetTransform.transform.position += movement;
    }

    void Move2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (true == (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)))
            {
                print("Clicked on: " + hit.collider.gameObject.name);

                // 클릭한 지점의 x, z 좌표로 캐릭터 이동
                clickTargetPos = new Vector3(hit.point.x, _targetTransform.position.y, hit.point.z);
                _targetTransform.transform.LookAt(clickTargetPos);
            }
        }

        Vector3 startPos = _targetTransform.transform.position;
        Vector3 dirVector = (clickTargetPos - startPos).normalized;

        if (Vector3.Distance(startPos, clickTargetPos) >= 0.01f)
        {
            _targetTransform.transform.position += dirVector * Time.deltaTime * movespeed;
        }

    }

    void LateUpdate()
    {

        if (!_targetTransform) return;

        // target rotation 
        //float currentAngle	= _cameraTransform.eulerAngles.y; //y angle
        //float targetAngle 	= _targetTransform.eulerAngles.y; 
        //currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angularSmoothTime, angularMaxSpeed);				
        //Quaternion currentRotation = Quaternion.Euler (fPitch, currentAngle + fYaw, 0);
        Quaternion currentRotation = Quaternion.Euler(fPitch, fYaw, 0);

        // target position
        Vector3 targetPos = _targetTransform.position + targetOffset;

        // camera position
        //_cameraTransform.position = targetPos + currentRotation * Vector3.back * fDistance; 
        fDistance_cur = Mathf.SmoothDamp(fDistance_cur, fDistance, ref fVelocity, fSmoothTime);
        _cameraTransform.position = targetPos + currentRotation * Vector3.back * fDistance_cur;

        // camera rotation
        Vector3 relativePos = targetPos - _cameraTransform.position;
        _cameraTransform.rotation = Quaternion.LookRotation(relativePos); // * Quaternion.Euler (fPitch, fYaw, 0 );	
    }
}
