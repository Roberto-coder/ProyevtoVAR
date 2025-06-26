using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float totalTime = 180f; // 3 minutos

    [Header("UI Final")]
    public TMP_Text finalScoreText;        // Referencia al TextMeshPro para mostrar el puntaje
    public Canvas finalCanvas;             // Canvas que contiene el puntaje final
    public string nextSceneName = "TestRoom"; // Nombre de la siguiente escena

    [Header("Audio Opcional")]
    public AudioSource backgroundMusic; // Si tienes música que debe detenerse al finalizar

    private bool gameEnded = false;

    void Start()
    {
        if (finalCanvas != null)
            finalCanvas.gameObject.SetActive(false); // Oculta el canvas final al inicio
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

        if (backgroundMusic != null)
            backgroundMusic.Stop();

        if (finalCanvas != null)
        {
            finalCanvas.gameObject.SetActive(true);

            if (finalScoreText != null)
                finalScoreText.text = "Puntuacion Final: " + ScoreManager.Instance.currentScore;
        }

        // Espera 5 segundos antes de cambiar de escena
        Invoke(nameof(LoadNextScene), 5f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
