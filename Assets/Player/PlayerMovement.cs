using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    private float yaw;
    private float pitch;
    [SerializeField] private float sensitivity = 150f;
    [SerializeField] private Transform mainCamera;
    
    [Header("Movement")]
    private Rigidbody rb;
    private Vector3 moveInput;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float airDamping = 3f;
    [SerializeField] private float groundDamping = 10f;

    [Header("Jump")]
    private bool isOnGround;
    private bool isOnSurface;
    private bool canJump;
    private bool jumpRequest;
    private Coroutine onAirC;
    

    private void Start()
    {
        // mainCamera = transform.GetChild(0).transform;

        rb = GetComponent<Rigidbody>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump) jumpRequest = true;
        
        yaw += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Y-axis rotation
        pitch += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // X-axis rotation
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        mainCamera.localRotation = Quaternion.Euler(-pitch, 0f, 0f);

        Vector3 forward = transform.forward; // Z‑axis of the player
        Vector3 right = transform.right; // X‑axis of the player

        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveInput = (forward * moveInput.z + right * moveInput.x).normalized;
    }
    
    // handle ground jumping
    private void FixedUpdate()
    {
        rb.AddForce(maxSpeed * moveInput, ForceMode.VelocityChange);
        
        if (isOnGround & jumpRequest)
        {
            jumpRequest = canJump = false;
            rb.linearDamping = airDamping;
            rb.AddForce(jumpSpeed * Vector3.up, ForceMode.VelocityChange);
        }
    }

    private IEnumerator Gravity()
    {
        while (!isOnGround || !isOnSurface)
        {
            yield return new WaitForFixedUpdate();
            rb.AddForce(10 * maxSpeed * Vector3.down, ForceMode.Acceleration);
        }
        onAirC = null;
    }
    
    private void OnCollisionEnter(Collision col)
    {
        // either on the ground, or on a surface
        if (col.gameObject.CompareTag("Ground"))
        {
            canJump = isOnGround = true;
            rb.linearDamping = groundDamping;
        }
        else
        {
            isOnSurface = true;
        }
        
        maxSpeed = rb.linearDamping / 5;
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground")) // if left the ground
        {
            isOnGround = canJump = false;
            if (!isOnSurface) Float(); // and if is not touching a wall, then float
        }
        
        else if (col.gameObject.CompareTag("Wall")) // if un-touched the wall
        {
            isOnSurface = false;
            if(!isOnGround) Float(); // and if is not on the ground, then float
        }
        
        maxSpeed = rb.linearDamping / 5;
    }

    private void Float()
    {
        // isOnGround = isOnSurface = false;
        rb.linearDamping = airDamping;
        onAirC ??= StartCoroutine(Gravity());
    }
}