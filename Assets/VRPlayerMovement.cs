using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.0f;
    public float rotationSpeed = 90.0f;
    public Transform cameraTransform;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public float groundedOffset = -0.14f;
    public LayerMask groundLayer;

    [Header("Audio")]
    public AudioSource footstepAudio;
    public float stepIntervalWalk = 0.6f;
    public float stepIntervalRun = 0.3f;

    private CharacterController controller;
    private float verticalVelocity;
    private float stepTimer = 0f;
    private bool isMoving = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (footstepAudio == null)
            Debug.LogWarning("Falta AudioSource asignado.");
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Mouse X");

        bool isRunning = Input.GetButton("Fire3"); // Shift en teclado, botón A en gamepad
        float speed = isRunning ? runSpeed : walkSpeed;

        // Movimiento en dirección de la cámara
        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0f;
        direction.Normalize();

        Vector3 move = direction * speed;

        // Aplicar rotación con joystick derecho
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);

        // Aplicar gravedad si no está tocando el suelo
        if (IsGrounded())
        {
            verticalVelocity = -1f; // lo mantiene pegado al suelo
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        isMoving = direction.magnitude > 0.1f && IsGrounded();

        HandleFootsteps(isRunning);
    }

    void HandleFootsteps(bool isRunning)
    {
        if (footstepAudio == null) return;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                footstepAudio.Play();
                stepTimer = isRunning ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    bool IsGrounded()
    {
        // Cast hacia abajo para detectar si toca el suelo
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(rayStart, Vector3.down * 0.3f, Color.green);
        return Physics.Raycast(rayStart, Vector3.down, 0.3f, groundLayer);
    }
}