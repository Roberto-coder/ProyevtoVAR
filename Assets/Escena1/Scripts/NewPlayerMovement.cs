using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 2f, runSpeed = 4f;
    public float rotationSpeed = 90f;
    public Transform cameraTransform;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    public LayerMask groundLayer;

    [Header("Raycast Mano Derecha")]
    public Transform handTransform; // ← Arrastra aquí la mano derecha
    public float rayDistance = 3f;
    public LayerMask treeLayer;

    [Header("Partículas")]
    public ParticleSystem aguaParticles;
    public ParticleSystem fertilizanteParticles;

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

        bool isRunning = Input.GetKey(KeyCode.JoystickButton2); // Botón X
        float baseSpeed = isRunning ? runSpeed : walkSpeed;

        float speed = PowerUpManager.Instance.speedBoost ? baseSpeed * 2f : baseSpeed;

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
                stepTimer = Input.GetKey(KeyCode.JoystickButton2) ? stepIntervalRun : stepIntervalWalk;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void HandleInteractions()
    {
        // Raycast desde la mano
        if (Physics.Raycast(handTransform.position, handTransform.forward, out RaycastHit hit, rayDistance, treeLayer))
        {
            TreeGrowth tree = hit.collider.GetComponent<TreeGrowth>();
            if (tree != null)
            {
                // BOTÓN A - Interactuar con árbol seco
                if (Input.GetKeyDown(KeyCode.JoystickButton0)) // A
                {
                    if (tree.IsDry)
                    {
                        tree.StartGrowth();
                    }
                }

                // GATILLO DERECHO - Agua
                if (Input.GetAxis("TriggersR") > 0.1f)
                {
                    if (!aguaParticles.isPlaying) aguaParticles.Play();
                    ScoreManager.Instance.AddPoints(5); // puntos por regar
                }
                else
                {
                    if (aguaParticles.isPlaying) aguaParticles.Stop();
                }

                // GATILLO IZQUIERDO - Fertilizante
                if (Input.GetAxis("TriggersL") > 0.1f)
                {
                    if (!fertilizanteParticles.isPlaying) fertilizanteParticles.Play();
                    ScoreManager.Instance.AddPoints(5); // puntos por fertilizar
                }
                else
                {
                    if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();
                }
            }
        }
        else
        {
            // No apuntando a árbol, detener partículas si están activas
            if (aguaParticles.isPlaying) aguaParticles.Stop();
            if (fertilizanteParticles.isPlaying) fertilizanteParticles.Stop();
        }
    }

    bool IsGrounded()
    {
        Vector3 start = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(start, Vector3.down, 0.3f, groundLayer);
    }
}
