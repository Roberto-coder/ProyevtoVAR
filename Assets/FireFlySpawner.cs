using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab;  // El objeto que quieres que se duplique
    public int amount = 5;           // Cu�ntas lib�lulas quieres
    public float spawnRadius = 5f;   // Qu� tan lejos del centro pueden aparecer

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            // Crea una posici�n aleatoria alrededor del objeto
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            pos.y = Mathf.Clamp(pos.y, 1f, 5f); // Controla la altura para que no est�n en el piso o muy arriba
            Instantiate(fireflyPrefab, pos, Quaternion.identity);
        }
    }
}
