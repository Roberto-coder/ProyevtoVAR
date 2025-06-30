using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMovement_Keyboard : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 2f, runSpeed = 4f;
    public float rotationSpeed = 90f;
    public Transform cameraTransform;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    [Header("Raycast Mano Derecha")]
    public Transform handTransform;
    public float rayDistance = 3f;
    public LayerMask treeLayer;

    [Header("Partículas")]
    public ParticleSystem aguaParticles;
    public ParticleSystem fertilizanteParticles;

    [Header("Audio")]
    public AudioSource footstepAudio;
    public float stepIntervalWalk = 0.6f, stepIntervalRun = 0.3f;

    [Header("Sonidos")]
    public AudioSource waterSound;
    public AudioSource fertilizerSound;

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
        HandleInteractions();
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S

        float rotH = 0f;
        if (Input.GetKey(KeyCode.Q)) rotH = -1f;
        else if (Input.GetKey(KeyCode.E)) rotH = 1f;

        transform.Rotate(Vector3.up, rotH * rotationSpeed * Time.deltaTime);

        Vector3 e = cameraTransform.localEulerAngles;
        float rotV = 0f;
        if (Input.GetKey(KeyCode.R)) rotV = 1f;
        else if (Input.GetKey(KeyCode.F)) rotV = -1f;
        e.x = Mathf.Clamp(e.x - rotV * rotationSpeed * Time.deltaTime, -80f, 80f);
        cameraTransform.localEulerAngles = e;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float baseSpeed = isRunning ? runSpeed : walkSpeed;
        float speed = PowerUpManager.Instance.speedBoost ? baseSpeed * 3f : baseSpeed;

        Vector3 dir = (cameraTransform.forward * v + cameraTransform.right * h);
        dir.y = 0;
        dir.Normalize();

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
                stepTimer = Input.GetKey(KeyCode.LeftShift) ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void HandleInteractions()
    {
        Debug.DrawRay(handTransform.position, handTransform.forward * rayDistance, Color.green);

        if (Physics.Raycast(handTransform.position, handTransform.forward, out RaycastHit hit, rayDistance, treeLayer))
        {
            TreeGrowth tree = hit.collider.GetComponent<TreeGrowth>();
            if (tree != null)
            {
                // 🌱 Plantar árbol seco (tecla Espacio)
                if (Input.GetKeyDown(KeyCode.Space) && tree.IsDry)
                    tree.StartGrowth();

                // 💧 Agua (click izquierdo)
                if (Input.GetMouseButton(0))
                {
                    if (!aguaParticles.isPlaying) aguaParticles.Play();
                    if (!waterSound.isPlaying) waterSound.Play();
                    ScoreManager.Instance.AddPoints(5);
                }
                else
                {
                    if (aguaParticles.isPlaying) aguaParticles.Stop();
                    if (waterSound.isPlaying) waterSound.Stop();
                }

                // 🧪 Fertilizante (click derecho)
                if (Input.GetMouseButton(1))
                {
                    if (!fertilizanteParticles.isPlaying) fertilizanteParticles.Play();
                    if (!fertilizerSound.isPlaying) fertilizerSound.Play();
                    ScoreManager.Instance.AddPoints(5);
                }
                else
                {
                    if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();
                    if (fertilizerSound.isPlaying) fertilizerSound.Stop();
                }
            }
        }
        else
        {
            // No apunta a árbol → detener efectos
            if (aguaParticles.isPlaying) aguaParticles.Stop();
            if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();

            if (waterSound.isPlaying) waterSound.Stop();
            if (fertilizerSound.isPlaying) fertilizerSound.Stop();
        }
    }

    bool IsGrounded()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(start, Vector3.down, 0.3f, groundLayer);
    }

    void OnDrawGizmos()
    {
        if (handTransform == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(handTransform.position, handTransform.position + handTransform.forward * rayDistance);
        Gizmos.DrawSphere(handTransform.position + handTransform.forward * rayDistance, 0.05f);
    }
}
/*
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMovement_Meta : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 2f, runSpeed = 4f;
    public float rotationSpeed = 90f;
    public Transform cameraTransform;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    [Header("Raycast Mano Derecha")]
    public Transform handTransform;
    public float rayDistance = 3f;
    public LayerMask treeLayer;

    [Header("Partículas")]
    public ParticleSystem aguaParticles;
    public ParticleSystem fertilizanteParticles;

    [Header("Audio")]
    public AudioSource footstepAudio;
    public float stepIntervalWalk = 0.6f, stepIntervalRun = 0.3f;

    [Header("Sonidos")]
    public AudioSource waterSound;
    public AudioSource fertilizerSound;

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
        HandleInteractions();
    }

    void HandleMovement()
    {
        float h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float v = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        Vector2 rot = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float rotH = rot.x;
        float rotV = rot.y;

        transform.Rotate(Vector3.up, rotH * rotationSpeed * Time.deltaTime);

        Vector3 e = cameraTransform.localEulerAngles;
        e.x = Mathf.Clamp(e.x - rotV * rotationSpeed * Time.deltaTime, -80f, 80f);
        cameraTransform.localEulerAngles = e;

        bool isRunning = OVRInput.Get(OVRInput.Button.Two); // Botón X
        float baseSpeed = isRunning ? runSpeed : walkSpeed;
        float speed = PowerUpManager.Instance.speedBoost ? baseSpeed * 3f : baseSpeed;

        Vector3 dir = (cameraTransform.forward * v + cameraTransform.right * h);
        dir.y = 0;
        dir.Normalize();

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
                stepTimer = OVRInput.Get(OVRInput.Button.Two) ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void HandleInteractions()
    {
        Debug.DrawRay(handTransform.position, handTransform.forward * rayDistance, Color.green);

        if (Physics.Raycast(handTransform.position, handTransform.forward, out RaycastHit hit, rayDistance, treeLayer))
        {
            TreeGrowth tree = hit.collider.GetComponent<TreeGrowth>();
            if (tree != null)
            {
                if (OVRInput.GetDown(OVRInput.Button.One) && tree.IsDry) // Botón A
                    tree.StartGrowth();

                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.1f)
                {
                    if (!aguaParticles.isPlaying) aguaParticles.Play();
                    if (!waterSound.isPlaying) waterSound.Play();
                    ScoreManager.Instance.AddPoints(5);
                }
                else
                {
                    if (aguaParticles.isPlaying) aguaParticles.Stop();
                    if (waterSound.isPlaying) waterSound.Stop();
                }

                if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.1f)
                {
                    if (!fertilizanteParticles.isPlaying) fertilizanteParticles.Play();
                    if (!fertilizerSound.isPlaying) fertilizerSound.Play();
                    ScoreManager.Instance.AddPoints(5);
                }
                else
                {
                    if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();
                    if (fertilizerSound.isPlaying) fertilizerSound.Stop();
                }
            }
        }
        else
        {
            if (aguaParticles.isPlaying) aguaParticles.Stop();
            if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();

            if (waterSound.isPlaying) waterSound.Stop();
            if (fertilizerSound.isPlaying) fertilizerSound.Stop();
        }
    }

    bool IsGrounded()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(start, Vector3.down, 0.3f, groundLayer);
    }

    void OnDrawGizmos()
    {
        if (handTransform == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(handTransform.position, handTransform.position + handTransform.forward * rayDistance);
        Gizmos.DrawSphere(handTransform.position + handTransform.forward * rayDistance, 0.05f);
    }
}
*/