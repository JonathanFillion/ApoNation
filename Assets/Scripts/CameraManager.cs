using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    private bool playerCameraEnabled = false;
    private bool vehiculeCameraEnabled = false;
    private bool tankCameraEnabled = false;
    private Camera playerCamera;
    private Camera vehiculeCamera;
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
        vehiculeCamera = GameObject.FindWithTag("VehiculeCamera").GetComponent<Camera>();
        tankCamera = GameObject.FindWithTag("TankCamera").GetComponent<Camera>();
        PlayerView();
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

    public void SwitchView()
    {
        if (playerCameraEnabled == true)
        {
            VehiculeView();
        }
        else
        {
            PlayerView();
        }
    }

    void TankView()
    {
        print("CaLLED");
        EnableTankCamera();
        DisableVehiculeCamera();
        DisablePlayerCamera();
        playerCameraEnabled = false;
        vehiculeCameraEnabled = false;
        tankCameraEnabled = true;
    }

    void PlayerView()
    {
        EnablePlayerCamera();
        DisableVehiculeCamera();
        DisableTankCamera();
        playerCameraEnabled = true;
        vehiculeCameraEnabled = false;
        tankCameraEnabled = false;
    }

    void VehiculeView()
    {
        DisablePlayerCamera();
        EnableVehiculeCamera();
        DisableTankCamera();
        playerCameraEnabled = false;
        vehiculeCameraEnabled = true;
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

    void EnableVehiculeCamera()
    {
        vehiculeCamera.enabled = true;
        vehiculeCamera.GetComponent<AudioListener>().enabled = true;
    }

    void DisableVehiculeCamera()
    {
        vehiculeCamera.enabled = false;
        vehiculeCamera.GetComponent<AudioListener>().enabled = false;
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
