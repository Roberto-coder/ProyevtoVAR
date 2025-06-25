using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float tiempoRestante = 10f;
    public float tiempoParpadepo = 5f;
    public TMP_Text textoTemporizador;

    private float blinkTimer = 0f;
    private float blinkInterval = 0.3f;
    public string sceneToLoad = "GameOverScene"; // Nombre de la escena a cargar al finalizar

    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
            textoTemporizador.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Parpadeo cuando quedan 5 segundos o menos
            if (tiempoRestante <= tiempoParpadepo)
            {
                blinkTimer += Time.deltaTime;
                if (blinkTimer >= blinkInterval)
                {
                    textoTemporizador.enabled = !textoTemporizador.enabled;
                    blinkTimer = 0f;
                }
            }
            else
            {
                textoTemporizador.enabled = true;
            }

            if (tiempoRestante <= 0)
            {
                textoTemporizador.enabled = true;
                textoTemporizador.text = "¡DONE!";
                // Espera 2 segundos y recarga la escena
                SceneManager.LoadScene(sceneToLoad);
                Invoke(sceneToLoad, 2f);
                
            }
        }
    }
}