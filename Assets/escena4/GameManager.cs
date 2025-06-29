using UnityEngine;
using TMPro; // ← importante para usar TextMeshPro
using UnityEngine.UI; // ← para manejar la UI (imagen)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreTMP;       // Texto de score en pantalla
    public GameObject logroUI;             // Imagen del logro final

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void IncreaseScore()
    {
        score++;
        Debug.Log("Score: " + score);

        // Actualizar el texto si está asignado
        if (scoreTMP != null)
        {
            scoreTMP.text = "SCORE : " + score.ToString();
        }

        // Si alcanza 2 puntos, muestra la imagen de logro
        if (score == 2 && logroUI != null)
        {
            logroUI.SetActive(true);
        }
    }
}
