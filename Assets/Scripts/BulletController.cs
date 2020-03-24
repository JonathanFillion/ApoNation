using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private bool hitOnce = false;

    public bool didDamage = false;

    private GameObject ImpactParticule;

    void Start()
    {
        hitOnce = false;
        didDamage = false;
        ImpactParticule = this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
    }

    void Update()
    {

    }

    public void doDamage()
    {
        this.didDamage = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitOnce == false)
        {
            hitOnce = true;
            var imp = ImpactParticule.GetComponent<ParticleSystem>();
            imp.Play();
        }

        if (collision.gameObject.tag != "Tank")
        {
            doDamage();
        }

    }
}
