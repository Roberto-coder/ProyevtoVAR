using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 180f; // 3 minutos
    public TMP_Text finalScoreText;
    public Canvas finalCanvas; // El canvas del resultado final
    public string nextSceneName = "TestRoom"; // o “Results”

    private bool gameEnded = false;

    void Start()
    {
        if (finalCanvas != null)
            finalCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        totalTime -= Time.deltaTime;
        if (totalTime <= 0f)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;

        // Mostrar el Canvas con puntuación
        if (finalCanvas != null)
        {
            finalCanvas.gameObject.SetActive(true);
            finalScoreText.text = "PUNTOS FINALES: " + ScoreManager.Instance.currentScore;
        }

        // Cambiar de escena luego de 5 segundos
        Invoke("LoadNextScene", 5f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
