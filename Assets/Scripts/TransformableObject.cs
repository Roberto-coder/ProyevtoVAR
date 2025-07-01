using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformableObject : MonoBehaviour
{
    [Header("Configuración de transformación")]
    public GameObject electricVersion;
    public int requiredMetal = 100;
    public int requiredComponents = 100;
    public int transformScore = 100;
    public int targetSceneIndex = 3; // Índice de escena en Build Settings

    private PromptUIController promptUI;
    private bool playerNear = false;

    void Start()
    {
        promptUI = GetComponentInChildren<PromptUIController>();
        if (promptUI == null)
            Debug.LogWarning("PromptUIController no asignado en el hijo del objeto.");
    }

    void Update()
    {
        //if (playerNear && Input.GetKeyDown(KeyCode.JoystickButton0)) // botón A en Xbox/Quest
        if (playerNear && OVRInput.GetDown(OVRInput.Button.One))
        {
            if (ResourceManager.TrySpendResources(requiredMetal, requiredComponents))
            {
                ResourceManager.UpdateScore(transformScore);

                float yOffset = 5.0f;
                Vector3 spawnPos = transform.position + Vector3.up * yOffset;
                Quaternion spawnRot = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                Instantiate(electricVersion, spawnPos, spawnRot);

                Destroy(gameObject);
                UIManager.Instance.RefreshUI();

                if (ResourceManager.Score >= 300)
                {
                    // Cambio de escena por índice
                    SceneManager.LoadScene(targetSceneIndex);
                }
            }
            else
            {
                Debug.Log("Necesitas más materiales.");
                promptUI.Show("Necesitas más materiales");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            playerNear = true;
            promptUI.Show("Presiona [A] para transformar");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            playerNear = false;
            promptUI.Hide();
        }
    }
}
