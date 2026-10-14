using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = 20f;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Camera Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float boundsMargin = 1.5f;

    private Rigidbody2D rb;
    private Camera cam;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        cam = Camera.main;
        verticalVelocity = 0f;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
        }

        float horizontalVelocity = moveInput.x * moveSpeed;

        // Проверка границ камеры
        if (useBounds && cam != null)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            Vector3 camPos = cam.transform.position;

            float minX = camPos.x - halfWidth + boundsMargin;
            float maxX = camPos.x + halfWidth - boundsMargin;

            float predictedX = rb.position.x + horizontalVelocity * Time.fixedDeltaTime;

            if (predictedX < minX || predictedX > maxX)
            {
                horizontalVelocity = 0f;
            }
        }

        rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
    }
}