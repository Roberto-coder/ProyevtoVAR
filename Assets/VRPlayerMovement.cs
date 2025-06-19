using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 2f, runSpeed = 4f;
    public float rotationSpeed = 90f;
    public Transform cameraTransform;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    [Header("Audio")]
    public AudioSource footstepAudio;
    public float stepIntervalWalk = 0.6f, stepIntervalRun = 0.3f;

    private CharacterController controller;
    private float verticalVelocity, stepTimer;
    private bool isMoving;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!footstepAudio) Debug.LogWarning("Asignar AudioSource para footsteps.");
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float rotH = Input.GetAxis("RightStickHorizontal");
        float rotV = Input.GetAxis("RightStickVertical");

        // Rotación horizontal del jugador
        transform.Rotate(Vector3.up, rotH * rotationSpeed * Time.deltaTime);

        // Rotación vertical de la cámara con límite
        Vector3 camEuler = cameraTransform.localEulerAngles;
        camEuler.x -= rotV * rotationSpeed * Time.deltaTime;
        camEuler.x = (camEuler.x > 180 ? camEuler.x - 360 : camEuler.x);
        camEuler.x = Mathf.Clamp(camEuler.x, -80f, 80f);
        cameraTransform.localEulerAngles = camEuler;

        bool isRunning = Input.GetButton("Fire3");
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 dir = cameraTransform.forward * v + cameraTransform.right * h;
        dir.y = 0; dir.Normalize();

        if (IsGrounded()) verticalVelocity = -1f;
        else verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = dir * speed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        isMoving = dir.magnitude > 0.1f && IsGrounded();
        HandleFootsteps(isRunning);
    }

    void HandleFootsteps(bool isRunning)
    {
        if (!footstepAudio) return;
        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstepAudio.Play();
                stepTimer = isRunning ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else stepTimer = 0f;
    }

    bool IsGrounded()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(start, Vector3.down * 0.3f, Color.green);
        return Physics.Raycast(start, Vector3.down, 0.3f, groundLayer);
    }
}
