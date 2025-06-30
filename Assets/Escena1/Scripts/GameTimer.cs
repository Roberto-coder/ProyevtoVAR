using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float totalTime = 60f; // 3 minutos

    [Header("UI Final")]
    public TMP_Text finalScoreText;
    public Canvas finalCanvas;

    [Header("Audio Opcional")]
    public AudioSource backgroundMusic;

    public SceneTransitionManager transitionManager;
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

    [System.Obsolete]
    void EndGame()
    {
        gameEnded = true;

        if (backgroundMusic != null)
            backgroundMusic.Stop();

        if (finalCanvas != null)
        {
            finalCanvas.gameObject.SetActive(true);

            if (finalScoreText != null && ScoreManager.Instance != null)
                finalScoreText.text = "Puntuación Final: " + ScoreManager.Instance.currentScore;
        }

        transitionManager.SafeLoadScene(1);
        //Invoke(nameof(LoadNextScene), 5f);
    }

    void LoadNextScene()
    {
        int nextSceneIndex = 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            try
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(" Error al cargar la escena por índice: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning(" No hay una escena en el índice: " + nextSceneIndex);
        }
    }
}
