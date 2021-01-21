using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    public CharacterController controller;
    protected enum CameraModeCode {Orbit,Walk};
    public float MoveSpeed = 1f;
    MyCameraControl childCamera;
    // Start is called before the first frame update
    void Start()
    {
        // get child ref
        controller = gameObject.GetComponentInChildren<CharacterController>();
        Debug.Log(controller);
        childCamera = gameObject.GetComponentInChildren<MyCameraControl>();
        Debug.Log(childCamera);
        Debug.Log(childCamera._CameraMode);
    }

    // Update is called once per frame
    void Update()
    {
        
        int mode = childCamera._CameraMode;
        if(mode == (int)CameraModeCode.Walk){

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * MoveSpeed);
        }
    }
}
