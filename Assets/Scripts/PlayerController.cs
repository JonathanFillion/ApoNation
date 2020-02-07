using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{

    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = new Vector3(0, 0, 0);
    Vector2 rotation = new Vector2(0, 0);

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }


    void Update()
    {

        CheckForVehiculeEntry();

        //What is Character Controller ?
        if (characterController.isGrounded)
        {
            //Normalized vectors responsible for direction
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            //Input keys detection
            float currentSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float currentSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            //Store current move detection with predefined speed on moveVector
            moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        
        characterController.Move(moveDirection * Time.deltaTime);
        //Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

    }

    void CheckForVehiculeEntry() {
        if (Input.GetKeyDown("e")) {
            CameraManager.instance.SwitchView();
        }
    }
}
