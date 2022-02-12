using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float playerJump;
    public GameObject Cam;
    public float mouseSens;

    private float minRot = -90.0f;
    private float maxRot = 90.0f;
    private Quaternion playerRot;
    private Quaternion camRot;


    private Rigidbody rb;
    private CapsuleCollider CC;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CC = GetComponent<CapsuleCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRot = transform.localRotation;
        camRot = Cam.transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        float mouseX = Input.GetAxis("Mouse X") * mouseSens;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        playerRot *= Quaternion.Euler(0, mouseX, 0);
        camRot *= Quaternion.Euler(-mouseY, 0, 0);

        transform.localRotation = playerRot;
        
        Cam.transform.localRotation = camRot;

        camRot = ClampRot(camRot);

    }

    private void Update()
    {
        PlayerJump();
        
    }

    void PlayerMovement()
    {
        float HMotion = Input.GetAxis("Horizontal") * playerSpeed;
        float VMotion = Input.GetAxis("Vertical") * playerSpeed;

        //transform.position += new Vector3(HMotion, 0, VMotion);

        transform.position += (Cam.transform.forward * VMotion) + (Cam.transform.right * HMotion);

    }

    void PlayerJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && OnGround())
        {
            rb.AddForce(0, playerJump, 0);
        }
    }

    bool OnGround()
    {
        RaycastHit PtoG;
        if (Physics.SphereCast(transform.position, CC.radius, Vector3.down, out PtoG, ((CC.height / 2.0f) - CC.radius + 0.1f)))
        {
            Debug.Log("Jumping");
            return true;

        }
        Debug.Log("Did not Jump");
        return false;
    }

    Quaternion ClampRot(Quaternion unClampedRot)
    {
        unClampedRot.x /= unClampedRot.w;
        unClampedRot.y /= unClampedRot.w;
        unClampedRot.z /= unClampedRot.w;

        unClampedRot.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(unClampedRot.x);
        angleX = Mathf.Clamp(angleX, minRot, maxRot);
        unClampedRot.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return unClampedRot;
    }

}
