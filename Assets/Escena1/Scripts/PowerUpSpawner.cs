using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefabs de Power-Ups")]
    public GameObject[] powerUpPrefabs;

    [Header("Área de Spawn (X, Y, Z)")]
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);

    [Header("Tiempo")]
    public float spawnInterval = 15f;

    private void Start()
    {
        InvokeRepeating("SpawnPowerUp", 5f, spawnInterval);
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefabs.Length == 0) return;

        // Elegir prefab al azar
        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject prefab = powerUpPrefabs[index];

        // Calcular posición aleatoria dentro del área
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        Instantiate(prefab, randomPos, Quaternion.identity);
    }

    // Visualización del área en Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + spawnAreaSize / 2f, spawnAreaSize);
    }
}
