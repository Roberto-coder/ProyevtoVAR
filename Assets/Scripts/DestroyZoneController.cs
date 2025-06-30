using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyZone : MonoBehaviour
{
    public Vacuum vacuum; // Asigna esto en el inspector
    public string sceneToLoad = "GameOverScene"; // Nombre de la escena a cargar al finalizar

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suckable"))
        {
            Debug.Log("Objeto entró en la zona de destrucción: " + other.name);
            SuckableItem item = other.GetComponent<SuckableItem>();
            Debug.Log("Estado de succión: " + vacuum.isSucking);
            if (item != null && vacuum.isSucking)
            {
                Debug.Log("Destruyendo objeto: " + item.itemType);
                ResourceManager.UpdateScore(item.itemValue);
                if (ResourceManager.Score == ResourceManager.MaxScore)
                {
                    Debug.Log("¡Has alcanzado el puntaje máximo!");
                    // Espera 2 segundos y recarga la escena
                    SceneManager.LoadScene(sceneToLoad);
                    Invoke(sceneToLoad, 2f);
                }
                Destroy(item.gameObject);
            }
        }
    }
}