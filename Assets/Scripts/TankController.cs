using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TankController : MonoBehaviour
{
    public float accelarationFactor = 100.0f;
    public float maximumSpeed = 10.0f;
    public float maximumTurningSpeed = 100.0f;
    public float rotationSpeed = 240.0f;
    public float frictionBasicFactor = 4.0f;
    public float lookSpeed = 2.0f;
    public float turnDragging = 10.0f;
    public float tankRecoilForce = 1000000;

    public Transform cameraParent;
    public GameObject bulletReference;

    private Vector2 cameraRotation = new Vector2(0, -90);
    private float lookXLimitMax = 13.0f;
    private float lookXLimitMin = 1.0f;
    private Rigidbody rb;
    private GameObject turret;
    private GameObject canon;
    private Camera camera;
    private bool enableFreeLook = false;

    private bool canonInRecoil = false;
    private float timeForcanonInRecoilRecovery = 0.0f;
    private float recoilRatio = 0.0f;
    private bool isGrounded;
    private RaycastHit hit;


    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        print(rb);
        turret = GameObject.FindWithTag("TankHeadContainer");
        canon = GameObject.FindWithTag("TankCanonContainer");
        Cursor.lockState = CursorLockMode.Locked;
        camera = GameObject.FindWithTag("TankCamera").GetComponent<Camera>();

    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            enableFreeLook = !enableFreeLook;
        }

        if (Input.GetMouseButtonDown(0) && canonInRecoil == false)
        {
            FireMainCanon();
        }


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
        print(cameraRotation.x);
        print(realAngle);
        /*Apply canon barrel inclination*/
        canon.transform.rotation = Quaternion.Euler(-realAngle, turret.transform.eulerAngles.y, turret.transform.eulerAngles.z);
    }

    public void FixedUpdate()
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
            notorque = false;
            if (rb.angularVelocity.magnitude < maximumTurningSpeed)
            {
                rb.AddTorque(Vector3.up * hin * rotationSpeed * rb.mass);
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

    void PerformRecoilRecoveryAnimation()
    {
        if (canonInRecoil == true)
        {
            timeForcanonInRecoilRecovery -= Time.deltaTime;
            if (timeForcanonInRecoilRecovery <= 0.0f && canonInRecoil == true)
            {
                canonInRecoil = false;
                return;
            }
            float recovery = 1 - (timeForcanonInRecoilRecovery / recoilRatio);
            float rem = recovery * recoilRatio;
            recoilRatio = recoilRatio - rem;
            print(recoilRatio);
            canon.transform.localPosition = canon.transform.localPosition + new Vector3(0, 0, rem);
        }
    }


    void FireMainCanon()
    {
        PerformCanonInRecoil();
        PerformTankBodyRecoil();
        PerformBullet();
    }

    void PerformBullet()
    {
        Instantiate(bulletReference, canon.transform.position, Quaternion.identity);
    }

    void PerformTankBodyRecoil()
    {
        rb.AddForce(turret.transform.TransformDirection(Vector3.back) * tankRecoilForce);
        //rb.AddTorque(turret.TransformDirection(Vector3.back) * tankRecoilForce);
    }

    void PerformCanonInRecoil()
    {
        recoilRatio = 2.0f;
        canon.transform.localPosition = canon.transform.localPosition + new Vector3(0, 0, -recoilRatio);
        canonInRecoil = true;
        timeForcanonInRecoilRecovery = 1.0f;

    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}