using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public bool doublePoints = false;
    public bool speedBoost = false;

    [Header("Lluvia")]
    public GameObject rainEffect; // Asigna el GameObject con partículas de lluvia
    public float lluviaPuntosIntervalo = 1f;
    public int puntosPorSegundo = 2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ActivatePower(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.DoublePoints:
                StartCoroutine(ApplyDoublePoints(duration));
                break;
            case PowerUpType.SpeedBoost:
                StartCoroutine(ApplySpeedBoost(duration));
                break;
            case PowerUpType.Rain:
                StartCoroutine(ApplyRain(duration));
                break;
        }
    }

    private IEnumerator ApplyDoublePoints(float duration)
    {
        doublePoints = true;
        Debug.Log("Puntos dobles activados");
        yield return new WaitForSeconds(duration);
        doublePoints = false;
        Debug.Log("Puntos dobles terminados");
    }

    private IEnumerator ApplySpeedBoost(float duration)
    {
        speedBoost = true;
        Debug.Log("Velocidad aumentada");
        yield return new WaitForSeconds(duration);
        speedBoost = false;
        Debug.Log("Velocidad normal");
    }

    private IEnumerator ApplyRain(float duration)
    {
        Debug.Log("Lluvia iniciada");

        if (rainEffect != null)
            rainEffect.SetActive(true); // Activar partículas

        float timer = 0f;
        while (timer < duration)
        {
            ScoreManager.Instance.AddPoints(puntosPorSegundo);
            yield return new WaitForSeconds(lluviaPuntosIntervalo);
            timer += lluviaPuntosIntervalo;
        }

        if (rainEffect != null)
            rainEffect.SetActive(false); // Desactivar lluvia

        Debug.Log("Lluvia terminada");
    }
}
