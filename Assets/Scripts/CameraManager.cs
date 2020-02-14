using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    Camera[] cameras;
    private bool playerCam = false;
    private bool vehiculeCam = false;
    private bool tankCam = false;
    
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
        cameras = Camera.allCameras;
        print(cameras.Length);
        PlayerView();
    }

    public void SwitchView()
    {
        if (playerCam)
        {
            print("Enable vehicule cam");

            VehiculeView();
        }
        else
        {
            print("Enable player cam");

            PlayerView();
        }
    }

    void TankView()
    {
        EnableTankCamera();
        DisableVehiculeCamera();
        DisablePlayerCamera();
        playerCam = false;
        vehiculeCam = false;
        tankCam = true;
    }

    void PlayerView()
    {
        EnablePlayerCamera();
        DisableVehiculeCamera();
        DisableTankCamera();
        playerCam = true;
        vehiculeCam = false;
        tankCam = true;
    }

    void VehiculeView()
    {
        DisablePlayerCamera();
        EnableVehiculeCamera();
        DisableTankCamera();
        playerCam = false;
        vehiculeCam = true;
    }

    void EnablePlayerCamera()
    {
        cameras[0].enabled = true;
        cameras[0].GetComponent<AudioListener>().enabled = true;
    }

    void DisablePlayerCamera()
    {
        cameras[0].enabled = false;
        cameras[0].GetComponent<AudioListener>().enabled = false;
    }

    void EnableVehiculeCamera()
    {
        cameras[1].enabled = true;
        cameras[1].GetComponent<AudioListener>().enabled = true;
    }

    void DisableVehiculeCamera()
    {
        cameras[1].enabled = false;
        cameras[1].GetComponent<AudioListener>().enabled = false;
    }

    void EnableTankCamera()
    {
        cameras[2].enabled = true;
        cameras[2].GetComponent<AudioListener>().enabled = true;
    }
    
    void DisableTankCamera()
    {
        cameras[2].enabled = false;
        cameras[2].GetComponent<AudioListener>().enabled = false;
    }
    
    void Update()
    {

    }
}
