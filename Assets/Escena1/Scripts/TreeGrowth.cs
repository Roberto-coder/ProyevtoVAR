using UnityEngine;
using System.Collections;

public class TreeGrowth : MonoBehaviour
{
    public bool IsDry = true; // Indica si es un árbol seco
    public float growthDuration = 2f;
    public int points = 10;

    [Header("Prefabs de árboles verdes")]
    public GameObject[] treesGreen; // ← Arrastra aquí los prefabs

    public void StartGrowth()
    {
        if (!IsDry || treesGreen.Length == 0) return;

        // Elegir un prefab aleatorio
        int index = Random.Range(0, treesGreen.Length);
        GameObject selectedPrefab = treesGreen[index];

        // Instanciar árbol verde
        GameObject newTree = Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation
        );

        // Escala inicial en 0 para animación de crecimiento
        newTree.transform.localScale = Vector3.zero;

        // Animar crecimiento
        StartCoroutine(GrowNewTree(newTree.transform));

        // Destruir este árbol seco
        Destroy(gameObject);
    }

    private IEnumerator GrowNewTree(Transform tree)
    {
        float elapsed = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 finalScale = Vector3.one;

        while (elapsed < growthDuration)
        {
            tree.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / growthDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tree.localScale = finalScale;

        ScoreManager.Instance.AddPoints(points);
    }
}
