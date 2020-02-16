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
        print("START IN ENTITY WORKS");
    }

    public void SwitchControls()
    {

        if (isPlayerControl == true)
        {
            TankEntityEnabled();
        }
        else
        {
            PlayerEntityEnabled();
        }
        
    }
    public void PlayerEntityEnabledAtStart()
    {
        isPlayerControl = true;
        isVehiculeControl = false;
        isTankControl = false;
        Player.SetActive(true);
        StartTimer();
    }

    public void PlayerEntityEnabled()
    {
        print("Should see to player");
        isPlayerControl = true;
        isVehiculeControl = false;
        isTankControl = false;
        Player.SetActive(true);
        StartTimer();
        //Player.transform.position = (Vehicule.transform.position + new Vector3(2, 0, 0));
    }

    public void VehiculeEntityEnabled()
    {
        isPlayerControl = false;
        isVehiculeControl = true;
        isTankControl = false;
        Player.SetActive(false);
        StartTimer();
    }

    public void TankEntityEnabled()
    {
        print("Should see to tank");
        isPlayerControl = false;
        isVehiculeControl = false;
        isTankControl = true;
        Player.SetActive(false);
        StartTimer();
    }

    private void StartTimer()
    {
        exitActionBlockingTimer = 1.0f;
        exitActionBlocked = true;
    }

    void Update()
    {
        exitActionBlockingTimer -= Time.deltaTime;
        if (exitActionBlockingTimer <= 0.0f && exitActionBlocked == true)
        {
            print("Timer finished");
            exitActionBlocked = false;
        }
    }
}
