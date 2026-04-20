using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    private bool isGrounded;
    private Transform spriteTransform;
    private Camera mainCamera;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        mainCamera = Camera.main;
        
        // Find sprite child
        if (transform.childCount > 0)
            spriteTransform = transform.GetChild(0);
        else
            Debug.LogWarning("No sprite child found! Add sprite as child of Player.", this);
        
        if (groundCheck == null)
            Debug.LogWarning("GroundCheck not assigned!", this);
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();
        
        Vector3 movement = (camRight * horizontal + camForward * vertical).normalized;
        
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
        
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        
        HandleSpriteDirection(movement);
    }
    
    void HandleSpriteDirection(Vector3 movement)
    {
        if (spriteTransform == null) return;
        
        // Only flip if there's actual horizontal movement
        if (Mathf.Abs(movement.x) > 0.01f)
        {
            Vector3 scale = spriteTransform.localScale;
            
            // Moving right - face right (positive X scale)
            if (movement.x > 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }
            // Moving left - face left (negative X scale)
            else if (movement.x < 0)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            
            spriteTransform.localScale = scale;
        }
    }
    
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}