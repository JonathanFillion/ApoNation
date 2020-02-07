using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]


public class VehiculeController : MonoBehaviour
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

    }


    void Update()
    {
        if (PresentEntitiesManager.instance.isVehiculeControl)
        {
            CheckForVehiculeExit();
            if (characterController.isGrounded)
            {
                //Normalized vectors responsible for direction
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                //Input keys detection
                float currentSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
                //Store current move detection with predefined speed on moveVector
                moveDirection = (forward * currentSpeedX);

                if (Input.GetButton("Jump") && canMove)
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            //Applying gravity
            moveDirection.y -= gravity * Time.deltaTime;
            //https://forum.unity.com/threads/adding-gravity-unity-c-basic-code.359966/
            characterController.Move(moveDirection * Time.deltaTime);
            transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
            //Camera rotation

            if (canMove)
            {
                rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
                rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
                playerCameraParent.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
                //transform.eulerAngles = new Vector2(0, rotation.y);
            }
        }
    }
    void CheckForVehiculeExit()
    {
        if (Input.GetKeyDown("e"))
        {
            CameraManager.instance.SwitchView();
            PresentEntitiesManager.instance.PlayerEntityEnabled();
        }
    }
}
