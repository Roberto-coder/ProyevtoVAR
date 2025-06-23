using UnityEngine;

public class TransformableObject : MonoBehaviour
{
    public GameObject electricVersion;
    public int requiredMetal = 100, requiredComponents = 100;
    public int transformScore = 100;
    PromptUIController promptUI;
    private bool playerNear = false;

    void Start()
    { promptUI = GetComponentInChildren<PromptUIController>(); }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.JoystickButton0)) // boton A en Quest
        {
            if (ResourceManager.TrySpendResources(requiredMetal, requiredComponents))
            {
                ResourceManager.UpdateScore(transformScore);

                // Dentro de tu script de transformación o disparo:
                float yOffset = 5.0f;  // Ajusta este valor en el Inspector

                Vector3 spawnPos = transform.position + new Vector3(0f, yOffset, 0f);
                Quaternion spawnRot = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                Instantiate(electricVersion, spawnPos, spawnRot);



                Destroy(gameObject);

                UIManager.Instance.RefreshUI();

                // Si Score ≥ 500, cambia de escena
                if (ResourceManager.Score >= 300)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene");
            }
            else
                Debug.Log("Necesitas más materiales.");
                promptUI.Show("Necesitas más materiales");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            playerNear = true;
            promptUI.Show("Presiona [A] para transformar");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            playerNear = false;
            promptUI.Hide();
    }
}
