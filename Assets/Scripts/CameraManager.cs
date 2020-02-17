using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    private bool playerCameraEnabled = false;
    private bool tankCameraEnabled = false;
    private Camera playerCamera;
    private Camera tankCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        tankCamera = GameObject.FindWithTag("TankCamera").GetComponent<Camera>();
        TankView();
    }

    public void SwitchViewTankPlayer()
    {
        if (playerCamera.enabled == true)
        {
            TankView();
        }
        else
        {
            print("WHY");
            PlayerView();
        }
    }

   

    void TankView()
    {
        print("CaLLED");
        EnableTankCamera();
        DisablePlayerCamera();
        playerCameraEnabled = false;
        tankCameraEnabled = true;
    }

    void PlayerView()
    {
        EnablePlayerCamera();
        DisableTankCamera();
        playerCameraEnabled = true;
        tankCameraEnabled = false;
    }


    void EnablePlayerCamera()
    {
        playerCamera.enabled = true;
        playerCamera.GetComponent<AudioListener>().enabled = true;
    }

    void DisablePlayerCamera()
    {
        playerCamera.enabled = false;
        playerCamera.GetComponent<AudioListener>().enabled = false;
    }

    void EnableTankCamera()
    {
        tankCamera.enabled = true;
        tankCamera.GetComponent<AudioListener>().enabled = true;
    }

    void DisableTankCamera()
    {
        tankCamera.enabled = false;
        tankCamera.GetComponent<AudioListener>().enabled = false;
    }

    void Update()
    {

    }
}
