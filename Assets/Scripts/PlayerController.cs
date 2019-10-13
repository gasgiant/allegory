using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody rb;
    public LayerMask groundLayer;
    public float rotationSpeed = 60;
    public float movementSpeed = 5;
    public bool grounded;
    public SaveZone saveZone;
    public CameraController cameraController;

    GameManager gm;

    float angVel;
    float targetAngvel;
    float angVelV;

    float hzVel;
    float targetHzVel;
    float hzVelV;

    int jumpCount;
    float nextTimeGrounded;

    void Start()
    {
        gm = GameManager.Instance;
    }

    void Update () 
    {
        if (gm.state == GameManager.GameState.Normal)
            HandleInput();

        angVel = Mathf.SmoothDamp(angVel, targetAngvel, ref angVelV, 0.2f);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, angVel * Time.deltaTime, 0));

        grounded = Grounded();

        if (grounded)
        {
            jumpCount = 0;
            hzVel = Mathf.SmoothDamp(hzVel, targetHzVel, ref hzVelV, 0.1f);
        }
        else
        {
            hzVel = Mathf.SmoothDamp(hzVel, targetHzVel, ref hzVelV, 0.5f);
        }

        rb.MovePosition(rb.position + transform.right * hzVel * Time.deltaTime);
            

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 && gm.state == GameManager.GameState.Normal)
        {
            jumpCount++;
            nextTimeGrounded = Time.time + 0.1f;
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.up * 15, ForceMode.VelocityChange);
            else
                rb.AddForce(Vector3.up * (15 - rb.velocity.y), ForceMode.VelocityChange);
        }

        if (rb.velocity.y > 0 )
        {
            if (Input.GetKey(KeyCode.Space))
                rb.AddForce(Vector3.down * 17, ForceMode.Acceleration);
            else
                rb.AddForce(Vector3.down * 45, ForceMode.Acceleration);
        }
        else
            if (rb.velocity.y > -100) rb.AddForce(Vector3.down * 50, ForceMode.Acceleration);
            
        if (transform.position.y < saveZone.deathLine)
        {
            rb.isKinematic = true;
            hzVel = 0;
            targetHzVel = 0;
            angVel = 0;
            targetAngvel = 0;
            cameraController.Reset(saveZone.position + saveZone.offset + Vector3.up * 30, Quaternion.Euler(0, 0, 0), rb.position);
            transform.position = saveZone.position + saveZone.offset;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.isKinematic = false;
        }
            
	}

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            targetAngvel = -rotationSpeed;
        if (Input.GetKeyUp(KeyCode.Q))
            targetAngvel = 0;
        if (Input.GetKeyDown(KeyCode.E))
            targetAngvel = rotationSpeed;
        if (Input.GetKeyUp(KeyCode.E))
            targetAngvel = 0;

        if (Input.GetKeyDown(KeyCode.A))
            targetHzVel = -movementSpeed;
        if (Input.GetKeyUp(KeyCode.A))
            targetHzVel = 0;
        if (Input.GetKeyDown(KeyCode.D))
            targetHzVel = movementSpeed;
        if (Input.GetKeyUp(KeyCode.D))
            targetHzVel = 0;
    }

    bool Grounded()
    {
        return nextTimeGrounded < Time.time && Physics.Raycast(new Ray(transform.position, Vector3.down), 0.65f, groundLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SaveZone")
        {
            saveZone = other.gameObject.GetComponent<SaveZone>();
        }
        if (other.tag == "Finish")
            GameManager.Instance.finished = true;
    }


}
