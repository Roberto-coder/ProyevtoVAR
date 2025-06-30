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
        if (!footstepAudio) Debug.LogWarning("Asigna AudioSource para footsteps.");
    }

    void Update()
    {
        HandleMovement();
        HandleFootsteps();
    }

    void HandleMovement()
    {
        // Lectura de ejes del Xbox (configurados en Input Manager)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float rotH = Input.GetAxis("RightStickHorizontal");
        float rotV = Input.GetAxis("RightStickVertical");
        
        /*float h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float v = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        // Entrada de rotación (joystick derecho)
        Vector2 rot = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float rotH = rot.x;
        float rotV = rot.y;*/

        // Rotación en yaw (giro horizontal del personaje)
        transform.Rotate(Vector3.up, rotH * rotationSpeed * Time.deltaTime);

        // Pitch de cámara (mirar arriba/abajo)
        Vector3 e = cameraTransform.localEulerAngles;
        e.x = Mathf.Clamp(e.x - rotV * rotationSpeed * Time.deltaTime, -80f, 80f);
        cameraTransform.localEulerAngles = e;

        // Determina si está corriendo
        bool isRunning = Input.GetButton("Fire3"); // Shift o botón A
        float speed = isRunning ? runSpeed : walkSpeed;

        // Movimiento horizontal relativo a cámara
        Vector3 dir = (cameraTransform.forward * v + cameraTransform.right * h);
        dir.y = 0;
        dir.Normalize();

        // Gravedad aplicada si no está en el suelo
        if (IsGrounded()) verticalVelocity = -1f;
        else verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = dir * speed + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);

        isMoving = dir.magnitude > 0.1f && IsGrounded();
    }

    void HandleFootsteps()
    {
        if (!footstepAudio) return;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {

                footstepAudio.PlayOneShot(footstepAudio.clip);
                stepTimer = Input.GetButton("Fire3") ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    bool IsGrounded()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        //Debug.DrawRay(start, Vector3.down * 0.3f, Color.green);
        return Physics.Raycast(start, Vector3.down, 0.3f, groundLayer);
    }
}