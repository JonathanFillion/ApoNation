using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCollider : MonoBehaviour
{

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        var isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.1f);
        print(isGrounded);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Ontrgienter");
    }

    private void OnTriggerExit(Collider other)
    {
        print("Ontrgiexit");

    }

}
