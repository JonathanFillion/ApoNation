using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class TankController : NetworkBehaviour
{
    public float accelarationFactor = 100.0f;
    public float maximumSpeed = 10.0f;
    public float maximumTurningSpeed = 100.0f;
    public float rotationSpeed = 240.0f;
    public float frictionBasicFactor = 4.0f;
    public float lookSpeed = 2.0f;
    public float turnDragging = 10.0f;
    public float tankRecoilForce = 1000000;
    public float bulletProjectileSpeed = 100; //000;

    public GameObject NetworkManager;


    public Transform cameraParent;
    public GameObject bulletReference;

    private int health;
    private bool alive;
    private Vector2 cameraRotation = new Vector2(0, -90);
    private float lookXLimitMax = 13.0f;
    private float lookXLimitMin = 1.0f;
    private Rigidbody rb;
    private GameObject turret;
    private GameObject canon;
    private GameObject canonTip;
    private GameObject canonSmoke;
    private GameObject deathGraySmoke;
    private GameObject deathBlackSmoke;
    private GameObject deathExplosion;
    private Camera camera;
    private bool enableFreeLook = false;

    private bool canonInRecoil = false;
    private float timeForCanonInRecoilRecovery = 0.0f;
    private float recoilRatio = 0.0f;
    private bool isGrounded;
    private RaycastHit hit;


    public void Start()
    {
        this.health = 100;
        this.alive = true;
        rb = transform.GetComponent<Rigidbody>();
        turret = transform.Find("TankHeadContainer").gameObject;
        canon = transform.Find("TankHeadContainer/TankCanonContainer").gameObject;
        canonTip = transform.Find("TankHeadContainer/TankCanonContainer/TankCanon/CanonTip").gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        camera = transform.Find("TankCameraParent/Camera").GetComponent<Camera>();
        canonSmoke = transform.Find("TankHeadContainer/TankCanonContainer/TankCanon/CanonTip/CanonParticle").gameObject;
        deathGraySmoke = transform.Find("DeathSmokeGray").gameObject;
        deathBlackSmoke = transform.Find("DeathSmokeBlack").gameObject;
        deathExplosion = transform.Find("DeathExplosion").gameObject;
    }

    public void Update()
    {

        if (!isLocalPlayer)
        {
            camera.enabled = false;
            camera.GetComponent<AudioListener>().enabled = false;

        }
        else

        if (isLocalPlayer)
        {
            if (Input.GetKeyDown("space"))
            {
                enableFreeLook = !enableFreeLook;
            }

            if (Input.GetMouseButtonDown(0) && canonInRecoil == false)
            {
                FireMainCanon();
            }




            ZoomCamera();
            AdjustCameraClipping();
            /*Camera Rotation*/
            cameraRotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            cameraRotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, lookXLimitMin, lookXLimitMax);
            //cameraRotation.y = Mathf.Clamp(cameraRotation.y, lookYLimitMin, lookYLimitMax);
            cameraParent.localRotation = Quaternion.Euler(0, cameraRotation.y, cameraRotation.x);

            /*Frontal point related to tank body*/
            float aimingDistanceTankBody = 100.0f;
            RaycastHit frontalOfTank;
            Vector3 frontalPoint = new Vector3();
            Ray tankBodyRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
            if (Physics.Raycast(tankBodyRay, out frontalOfTank))
            {
                frontalPoint = frontalOfTank.point;
                //Debug.DrawLine(transform.position, frontalOfTank.point, Color.red);
            }
            else
            {
                frontalPoint = tankBodyRay.origin + tankBodyRay.direction * aimingDistanceTankBody;
                //Debug.DrawLine(tankBodyRay.origin, frontalPoint, Color.red);
            }

            /*Canon Rotation*/
            RaycastHit hit;
            float aimingDistance = 500.0f;
            Vector3 aimPoint = new Vector3();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                aimPoint = hit.point;
                //Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
            else
            {
                aimPoint = ray.origin + ray.direction * aimingDistance;
                //Debug.DrawLine(ray.origin, aimPoint, Color.red);
            }

            /*Turret rotation*/
            var tempAimPoint = aimPoint;
            tempAimPoint.y = transform.position.y;
            if (enableFreeLook == false)
            {
                turret.transform.LookAt(tempAimPoint);
            }

            /*Calculate inclination angle for canon*/
            float inclinationRatio = (cameraRotation.x - 1) / 14;
            float canonAngle = 7.5f;
            float calcAngle = inclinationRatio * canonAngle;
            float realAngle = calcAngle;
            /*Apply canon barrel inclination*/
            canon.transform.rotation = Quaternion.Euler(-realAngle, turret.transform.eulerAngles.y, turret.transform.eulerAngles.z);
        }
    }


    public void FixedUpdate()
    {
        if (isLocalPlayer && alive)
        {
            float speed = rb.velocity.magnitude;
            var velocity = rb.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var velocityAngle = Vector3.Angle(rb.velocity, transform.TransformDirection(Vector3.forward)) - 90;
            bool notorque = true;
            float traction = 1 / speed;
            float vin = Input.GetAxis("Vertical");
            float hin = Input.GetAxis("Horizontal");


            /*Vertical*/
            if (vin != 0 && isGrounded == true)
            {
                notorque = false;
                if (localVelocity.z > -maximumSpeed && localVelocity.z < maximumSpeed)
                {
                    rb.AddForce(transform.TransformDirection(Vector3.forward) * vin * accelarationFactor * rb.mass);
                }
            }

            /*Rotation*/
            if (hin != 0 && isGrounded == true)
            {
                if (vin >= 0)
                {
                    notorque = false;
                    if (rb.angularVelocity.magnitude < maximumTurningSpeed)
                    {
                        rb.AddTorque(Vector3.up * hin * rotationSpeed * rb.mass);
                    }
                }
                else if (vin < 0)
                {
                    notorque = false;
                    if (rb.angularVelocity.magnitude < maximumTurningSpeed)
                    {
                        rb.AddTorque(Vector3.up * -hin * rotationSpeed * rb.mass);
                    }

                }
            }
            else
            {
                rb.AddTorque(-rb.angularVelocity * turnDragging * rb.mass);
            }

            /*Friction*/
            float friction = 0f;
            var frictionAngle = Vector3.Angle(localVelocity, -Vector3.forward);
            if (frictionAngle < 90)
            {
                friction = frictionAngle;
            }
            else if (frictionAngle > 90)
            {
                friction = Mathf.Abs(frictionAngle - 180);
            }
            if (rb.velocity.magnitude >= 0.5 && isGrounded == true)
            {
                Vector3 negativeNormalizedVelocity = -rb.velocity.normalized;
                rb.AddForce(negativeNormalizedVelocity * friction * frictionBasicFactor);
            }

            /*Slerp*/
            /* https://docs.unity3d.com/ScriptReference/Vector3.Slerp.html */
            if (velocityAngle > 0 && notorque == false && isGrounded == true)
            {
                rb.velocity = Vector3.Slerp(rb.velocity, -transform.forward * rb.velocity.magnitude, traction);
            }
            else if (velocityAngle < 0 && notorque == false && isGrounded == true)
            {
                rb.velocity = Vector3.Slerp(rb.velocity, transform.forward * rb.velocity.magnitude, traction);
            }
            PerformRecoilRecoveryAnimation();
        }
    }

    void AdjustCameraClipping()
    {
        float distance = Vector3.Distance(transform.position, camera.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, camera.transform.position, out hit, distance + 0.5f))
        {
            if (hit.collider.tag != "Tank")
            {

            }
        }
    }

    void ZoomCamera()
    {
        var minFov = 1;
        var maxFov = 100;
        float fov = camera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * -10;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        camera.fieldOfView = fov;
    }

    void PerformRecoilRecoveryAnimation()
    {
        if (canonInRecoil == true)
        {
            timeForCanonInRecoilRecovery -= Time.deltaTime;
            if (timeForCanonInRecoilRecovery <= 0.0f && canonInRecoil == true)
            {
                canonInRecoil = false;
                return;
            }
            float recovery = 1 - (timeForCanonInRecoilRecovery / recoilRatio);
            float rem = recovery * recoilRatio;
            recoilRatio = recoilRatio - rem;
            canon.transform.localPosition = canon.transform.localPosition + new Vector3(0, 0, rem);
        }
    }

    void FireMainCanon()
    {
        PerformCanonInRecoil();
        PerformTankBodyRecoil();
        CmdPerformBullet();
        PerformCanonSmoke();
    }

    void PerformCanonSmoke()
    {
        var smk = canonSmoke.GetComponent<ParticleSystem>();
        smk.Play();
    }

    void PerformDeathAnimation()
    {
            var de = deathExplosion.GetComponent<ParticleSystem>();
            var dbs = deathBlackSmoke.GetComponent<ParticleSystem>();
            var dgs = deathGraySmoke.GetComponent<ParticleSystem>();
            de.Play();
            dbs.Play();
            dgs.Play();
            Debug.Log("Should explode");
    }


    //This is where you would do stuffs that the client wants the server to do.
    [Command]
    void CmdPerformBullet()
    {
        var bullet = Instantiate(bulletReference);
        var rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(canon.transform.TransformDirection(Vector3.forward) * rbBullet.mass * bulletProjectileSpeed);
        NetworkServer.Spawn(bullet);//, connectionToClient);
        ServerBullet(bullet, canonTip.transform.position, canon.transform.rotation);
        //SpawnBullet(canonTip.transform.position, canon.transform.rotation, canon.transform.TransformDirection(Vector3.forward));
    }

    [ServerCallback]
    public void ServerBullet(GameObject go, Vector3 position, Quaternion rotation)
    {
        go.transform.position = position;
        go.transform.rotation = rotation;
    }

    [ClientCallback]
    public void ClientCallback()
    {

    }

    [ClientRpc]
    void RpcClientPerformBullet()
    {
        var bullet = Instantiate(bulletReference, canonTip.transform.position, canon.transform.rotation);
        var rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(canon.transform.TransformDirection(Vector3.forward) * rbBullet.mass * bulletProjectileSpeed);
        NetworkServer.Spawn(bullet, connectionToClient);
    }

    private void doTankDamage(int damage)
    {
        this.health -= damage;
        if (this.health < 0 && alive)
        {
            this.alive = false;
            PerformDeathAnimation();
        }
    }

    void PerformTankBodyRecoil()
    {
        rb.AddForce(turret.transform.TransformDirection(Vector3.back) * tankRecoilForce);
    }

    void PerformCanonInRecoil()
    {
        recoilRatio = 2.0f;
        canon.transform.localPosition = canon.transform.localPosition + new Vector3(0, 0, -recoilRatio);
        canonInRecoil = true;
        timeForCanonInRecoilRecovery = 1.5f;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "BulletTank")
        {
            var bulletManager = collision.gameObject.GetComponent<BulletController>();
            if (bulletManager.didDamage == false)
            {
                bulletManager.doDamage();
                doTankDamage(34);
            }
        }

    }



    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = false;
        }
    }
}