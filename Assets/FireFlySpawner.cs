using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab;  // El objeto que quieres que se duplique
    public int amount = 5;           // Cuántas libélulas quieres
    public float spawnRadius = 5f;   // Qué tan lejos del centro pueden aparecer

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            // Crea una posición aleatoria alrededor del objeto
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            pos.y = Mathf.Clamp(pos.y, 1f, 5f); // Controla la altura para que no estén en el piso o muy arriba
            Instantiate(fireflyPrefab, pos, Quaternion.identity);
        }
    }
}
