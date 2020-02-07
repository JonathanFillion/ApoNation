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
    private GameObject Player;
    private GameObject Vehicule;

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
        PlayerEntityEnabledAtStart();
    }

    public void SwitchControls()
    {
        if (isPlayerControl)
        {
            VehiculeEntityEnabled();
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
        Player.SetActive(true);
    }

    public void PlayerEntityEnabled()
    {
        isPlayerControl = true;
        isVehiculeControl = false;
        Player.transform.position = Vehicule.transform.position;
        Player.SetActive(true);
    }

    public void VehiculeEntityEnabled()
    {
        isPlayerControl = false;
        isVehiculeControl = true;
        Player.SetActive(false);
    }

    void Update()
    {

    }
}
