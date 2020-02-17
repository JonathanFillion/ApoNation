using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TankController : MonoBehaviour
{
    public float accelaration = 10.0f;
    public float maximumSpeed = 10.0f;
    public float maximumTurningSpeed = 1.0f;
    public float angularAcceleration = 10.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public float turnDragging = 10.0f;
    public Transform cameraParent;
    public float DistanceToTheGround;

    private Vector2 cameraRotation = new Vector2(0, -90);
    private Rigidbody rb;

    //Reusability
    private bool isGrounded;
    private RaycastHit hit;


    public void Start()
    {
        DistanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

        float speed = rb.velocity.magnitude;
        var velocity = rb.velocity;
        var localVel = transform.InverseTransformDirection(velocity);
        var velocityAngle = Vector3.Angle(rb.velocity, transform.TransformDirection(Vector3.forward)) - 90;
        bool notorque = true;
        float traction = 1 / speed;
        float vin = Input.GetAxis("Vertical");
        float hin = Input.GetAxis("Horizontal");
        //isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, DistanceToTheGround + 0.1f);
        //print(DistanceToTheGround);
        //print(isGrounded);

        if (vin != 0 && isGrounded == true)
        {
            notorque = false;
            if (localVel.z > -maximumSpeed && localVel.z < maximumSpeed)
            {
                rb.AddForce(transform.TransformDirection(Vector3.forward) * vin * accelaration * rb.mass);
            }
        }




        if (hin != 0 && isGrounded == true)
        {
            notorque = false;
            if (rb.angularVelocity.magnitude < maximumTurningSpeed)
            {
                rb.AddTorque(Vector3.up * hin * angularAcceleration * rb.mass);
            }
        }
        else
        {
            rb.AddTorque(-rb.angularVelocity * turnDragging * rb.mass);
        }

        if (velocityAngle > 0 && notorque == false && isGrounded == true)
        {
            rb.velocity = Vector3.Slerp(rb.velocity, -transform.forward * rb.velocity.magnitude, traction);
        }
        else if (velocityAngle < 0 && notorque == false && isGrounded == true)
        {
            rb.velocity = Vector3.Slerp(rb.velocity, transform.forward * rb.velocity.magnitude, traction);
        }
        //Camera Rotation
        /*cameraRotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        cameraRotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -lookXLimit, lookXLimit);
        cameraParent.localRotation = Quaternion.Euler(0, cameraRotation.y, cameraRotation.x);*/

    }

    void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }


    void CheckForVehiculeExit()
    {
        if (Input.GetKeyDown("e") && !PresentEntitiesManager.instance.exitActionBlocked)
        {
        }
    }
}