using UnityEngine;

public class MyCameraControl : MonoBehaviour
{

    protected enum CameraModeCode {Orbit,Walk};
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDefaultDistance = 10f;
    protected float _CameraDistance = 0f;

    // public int _CameraMode = (int)CameraModeCode.Walk;
    public int _CameraMode = (int)CameraModeCode.Orbit;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitSpeed = 10f;
    public float ScrollSpeed = 6f;
    public float MoveSpeed = 0.05f;
    public bool CameraDisabled = false;


    private bool _bMouseHold = false;
    private bool _bInputCap = true;


    // private Vector3 _CameraTargetPositionMark;


    Vector3 getCameraTargetPositionMark(string name){
        Vector3 res;
        res = GameObject.Find(name).transform.position;
        return res;
    }

    void setCameraTargetPosition(Vector3 position){
        setParentPosition(position);
    }

    void setParentPosition(Vector3 position){
        this._XForm_Parent.position = position;

    }

    Vector3 getCameraCurrentPosition(){
        Vector3 position = this._XForm_Parent.position;
        Debug.Log(position);;
        return position;
    }

    void SetInputCap(int val){
        if(val > 0){
            this._bInputCap = true;
        }else{
            this._bInputCap = false;
        }
    }

    
    void SetCameraDisable(int val){
    // mouse not over unity canvas
        if (val > 0)
        {
            CameraDisabled = true;
        }else{
            CameraDisabled = false;
        }
    }

    void SetCameraDistance(float val){
        this._CameraDistance = val;
    }

    void SetCameraMode(int val){
        this._CameraMode = val;
    }

    int getCameraMode(){
        return this._CameraMode;
    }
    // Start is called before the first frame update
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
        // _CameraMode = (int)CameraModeCode.Orbit;
        _CameraMode = (int)CameraModeCode.Walk;
        Debug.Log("camera mode");
        Debug.Log(_CameraMode);

        Vector3 initCameraPosition = getCameraTargetPositionMark("CameraPositionMark1");
        setCameraTargetPosition(initCameraPosition);

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            this._bMouseHold = true;
            Debug.Log("Mouse Hold");
        // }else{

        } else if (Input.GetMouseButtonUp(0)){

            this._bMouseHold = false;
            Debug.Log("Mouse Not Hold");
        }

    }
    // late Update is called once per frame after update()
    void LateUpdate()
    {
        // pause game for ignore key input
        if(CameraDisabled){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }

        if(this._CameraMode == (int)CameraModeCode.Orbit){
            // switch from walk mode as distance is 0
            if(this._CameraDistance == 0){
                this._CameraDistance = this._CameraDefaultDistance;

                // reset parent target cube position
                this._XForm_Parent.position = new Vector3(0f,0f,0f);

            }
            SetCameraDistance(this._CameraDistance);

            if(!CameraDisabled & this._bMouseHold){
                if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
                    _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                    _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                    // // clamp the y rotation not flipping
                    // if(_LocalRotation.y < 0f)
                    //     _LocalRotation = 0f;
                    // else if(_LocalRotation.y > 90f)
                    //     _LocalRotation.y = 90f;

                    _LocalRotation.y = Mathf.Clamp(_LocalRotation.y,0f,90f);
                    
                }
            }

            if(!CameraDisabled & this._bInputCap){
                // Zooming input from mouse scrool wheel or slide control  -- TODO
                if(Input.GetAxis("Mouse ScrollWheel") != 0f){
                    float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
                    // makes camera zoom faster the futher away from the target
                    ScrollAmount *= (this._CameraDistance * 0.3f);

                    this._CameraDistance += ScrollAmount * -1f;
                    // clamp camera distance to the target in meters
                    this._CameraDistance = Mathf.Clamp(this._CameraDistance,1.5f,1000f);

                }
            }


        }

        if(this._CameraMode == (int)CameraModeCode.Walk){
            SetCameraDistance(0);

            if(!CameraDisabled & this._bMouseHold){
                if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
                    _LocalRotation.x -= Input.GetAxis("Mouse X") * MouseSensitivity;
                    _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

                    // walk mode needs look from botton to top 
                    _LocalRotation.y = Mathf.Clamp(_LocalRotation.y,-90f,90f);
                    
                }

            }

        }
        // do the transform to camera
        Quaternion QT = Quaternion.Euler(_LocalRotation.y,_LocalRotation.x,0);
        
        // animate to rotate the target A.K.A the camera
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation,QT,Time.deltaTime * OrbitSpeed);

        if(this._XForm_Camera.localPosition.z != this._CameraDistance * -1f){
            this._XForm_Camera.localPosition = new Vector3(0f,0f,Mathf.Lerp(this._XForm_Camera.localPosition.z,this._CameraDistance * -1f,Time.deltaTime * ScrollSpeed));

        }


    }
}
