using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 CenterOfMass;

    [Range(0, 1)]
    public float throttle;

    public float speed = 0f;
    public float rollSpeed = 0f;
    public float turnSpeed = 0f;
    public float fuel = 0f;
    public bool invertY = false;
    public int angularDrag = 1;
    public bool sas = false;
    public List<ParticleSystem> forward;
    public List<ParticleSystem> backward;
    public List<ParticleSystem> left;
    public List<ParticleSystem> right;
    public List<ParticleSystem> up;
    public List<ParticleSystem> down;
    public List<ParticleSystem> rollLeft;
    public List<ParticleSystem> rollRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMass;
    }

    private void OnEnable()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        Throttle();
        Sas();
        if (fuel > 0)
        {
            rb.AddRelativeForce(Force(), ForceMode.VelocityChange);
            if (Input.GetKey(KeyCode.Mouse1))
            {
                rb.AddRelativeTorque(Torque(), ForceMode.VelocityChange);
            }
        }
    }

    private Vector3 Force()
    {
        var force = new Vector3() * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.forward * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(Vector3.back * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeForce(Vector3.left * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(Vector3.right * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.R))
        {
            rb.AddRelativeForce(Vector3.up * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.F))
        {
            rb.AddRelativeForce(Vector3.down * speed * throttle);
            fuel = fuel - 0.01f * throttle;
        }
        return force;
    }

    private Vector3 Torque()
    {
        float yaw = Input.GetAxis("Mouse X");
        //rb.AddRelativeTorque(transform.up * turnSpeed * yaw);

        float pitch = Input.GetAxis("Mouse Y") * (invertY ? 1 : -1);
        //rb.AddRelativeTorque(transform.up * turnSpeed * pitch);
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddRelativeTorque(Vector3.forward * rollSpeed);
            fuel = fuel - 0.01f * throttle;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rb.AddRelativeTorque(Vector3.back * rollSpeed);
            fuel = fuel - 0.01f * throttle;
        }
        return new Vector3(pitch, yaw) * turnSpeed;
    }

    private void Throttle()
    {
        throttle += Input.GetAxis("Mouse ScrollWheel");
        throttle = Mathf.Clamp(throttle, 0f, 1f);
    }

    private void Sas()
    {
        if (sas == false && Input.GetKeyDown(KeyCode.T))
        {
            rb.angularDrag = angularDrag;
            sas = true;
        }
        else if (sas == true && Input.GetKeyDown(KeyCode.T))
        {
            rb.angularDrag = 0f;
            sas = false;
        }
    }
}