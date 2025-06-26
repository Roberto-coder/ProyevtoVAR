using UnityEngine;

public class TreeHitDestroy : MonoBehaviour
{
    [Header("Configuraci�n de �rbol")]
    public string targetTag = "arbol";     // Tag identificador
    public int maxHits = 3;               // Golpes necesarios
    public int scorePerDestruction = 50;  // Puntos por destruir

    private int currentHits = 0;

    void OnCollisionEnter(Collision col)
    {
        // Solo cuenta golpes si choca con objeto etiquetado "arbol"
        if (col.gameObject.CompareTag(targetTag))
        {
            currentHits++;
            Debug.Log($"�rbol golpeado ({currentHits}/{maxHits})");

            if (currentHits >= maxHits)
            {
                // Usamos el singleton correctamente
                if (TreeResourceManager.Instance != null)
                    TreeResourceManager.Instance.AddTree(scorePerDestruction);

                Destroy(col.gameObject);
                Debug.Log($"�rbol destruido. Puntos otorgados: {scorePerDestruction}");
            }
        }
    }
}
