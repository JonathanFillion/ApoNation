using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TankController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBraqueTorque;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public Transform cameraParent;

    private Vector2 cameraRotation = new Vector2(0, 0);
    private Rigidbody rb;
    private bool highone = false;
    private bool lowone = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (PresentEntitiesManager.instance.isTankControl)
        {
            CheckForVehiculeExit();
        }
    }

    public void FixedUpdate()
    {

        //if (PresentEntitiesManager.instance.isTankControl)
       // {
            //CheckForVehiculeExit();
        /*    float speed = rb.velocity.magnitude;
            var velocity = rb.velocity;
            var localVel = transform.InverseTransformDirection(velocity);
            float motor = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
            float steering = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
            float brake = 0;

            if (localVel.z > 0)
            {
                brake = maxBraqueTorque * -1 * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);
            }
            else
            {
                brake = maxBraqueTorque * Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
            }

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering * maxSteeringAngle;
                    axleInfo.rightWheel.steerAngle = steering * maxSteeringAngle; ;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = maxMotorTorque * motor;
                    axleInfo.rightWheel.motorTorque = maxMotorTorque * motor;
                }


                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }*/

            

            //Camera Rotation
            cameraRotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            cameraRotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -lookXLimit, lookXLimit);
            cameraParent.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
       // }
       // else
      //  {
      //      rb.drag = 10;
     //   }
    }

    void CheckForVehiculeExit()
    {
        if (Input.GetKeyDown("e") && !PresentEntitiesManager.instance.exitActionBlocked)
        {
            print("e");
            PresentEntitiesManager.instance.PlayerEntityEnabled();
            CameraManager.instance.SwitchView();
        }
    }
}