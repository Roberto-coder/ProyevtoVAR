using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Prefab de basura")]
    public GameObject[] trashPrefabs;

    [Header("Rango de generaci�n")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(50, 0, 50);

    [Header("Cantidad")]
    public int trashAmount = 20;

    void Start()
    {
        SpawnTrash();
    }

    void SpawnTrash()
    {
        for (int i = 0; i < trashAmount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
            Instantiate(prefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float z = Random.Range(-areaSize.z / 2, areaSize.z / 2);
        float y = areaCenter.y;

        return new Vector3(x + areaCenter.x, y, z + areaCenter.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
