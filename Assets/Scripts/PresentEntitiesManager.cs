using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEntitiesManager : MonoBehaviour
{

    public static PresentEntitiesManager instance = null;

    [HideInInspector]
    public bool isPlayerControl = false;
    [HideInInspector]
    public bool isVehiculeControl = false;
    public bool isTankControl = false;
    public float exitActionBlockingTimer = 0.0f;
    public bool exitActionBlocked = false;
    private GameObject Player;
    private GameObject Vehicule;
    private GameObject Tank;

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
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Vehicule = GameObject.FindWithTag("Vehicule");
        Tank = GameObject.FindWithTag("Tank");
        PlayerEntityEnabledAtStart();
    }

    public void SwitchControls()
    {

        if (isPlayerControl)
        {
            TankEntityEnabled();
        }
        else
        {
            PlayerEntityEnabled();
        }
        exitActionBlockingTimer = 1.0f;
    }
    public void PlayerEntityEnabledAtStart()
    {
        isPlayerControl = true;
        isVehiculeControl = false;
        Player.SetActive(true);
    }

    public void PlayerEntityEnabled()
    {
        isPlayerControl = true;
        isVehiculeControl = false;
        Player.SetActive(true);
        Player.transform.position = (Vehicule.transform.position + new Vector3(2, 0, 0));
    }

    public void VehiculeEntityEnabled()
    {
        isPlayerControl = false;
        isVehiculeControl = true;
        Player.SetActive(false);
    }

    public void TankEntityEnabled()
    {
        isPlayerControl = false;
        isVehiculeControl = false;
        isTankControl = true;
        Player.SetActive(false);
    }

    void Update()
    {
        exitActionBlockingTimer -= Time.deltaTime;
        if (exitActionBlockingTimer <= 0.0f && exitActionBlocked == true)
        {
            exitActionBlocked = false;
        }
    }
}
