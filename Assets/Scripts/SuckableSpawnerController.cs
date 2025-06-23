using UnityEngine;

public class SuckableItemSpawner : MonoBehaviour
{
    [Header("Configuración de área")]
    public Vector2 spawnAreaSize = new Vector2(50f, 50f); // X/Z
    public int numberOfIslands = 5;
    public int itemsPerIsland = 10;
    public float islandRadius = 5f;

    [Header("Prefabs")]
    public GameObject suckablePrefab; // Prefab base con script SuckableItem

    void Start()
    {
        SpawnIslands();
    }

    void SpawnIslands()
    {
        for (int i = 0; i < numberOfIslands; i++)
        {
            Vector3 islandCenter = GetRandomPositionInArea();

            for (int j = 0; j < itemsPerIsland; j++)
            {
                Vector3 offset = Random.insideUnitSphere * islandRadius;
                offset.y = 0f; // solo en el plano XZ
                Vector3 spawnPos = islandCenter + offset;

                GameObject instance = Instantiate(suckablePrefab, spawnPos, Quaternion.identity);

                // La aleatoriedad del tipo y modelo ocurre dentro del prefab (como en tu versión previa)
            }
        }
    }

    Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        float y = 1f; // terreno plano, ajustar si tienes altura

        return new Vector3(x, y, z) + transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 areaCenter = transform.position;
        Gizmos.DrawWireCube(areaCenter, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
    }
}