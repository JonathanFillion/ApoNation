using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    Camera[] cameras;
    private bool playerCam = false;
    private bool vehiculeCam = false;

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
        PlayerView();
    }

    public void SwitchView()
    {
        if (playerCam)
        {
            VehiculeView();
        }
        else
        {
            PlayerView();
        }
    }

    void PlayerView()
    {
        EnablePlayerCamera();
        DisableVehiculeCamera();
        playerCam = true;
        vehiculeCam = false;
    }

    void VehiculeView()
    {
        DisablePlayerCamera();
        EnableVehiculeCamera();
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

    void Update()
    {

    }
}
