using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public void SafeLoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneSafely(sceneIndex));
    }

    private IEnumerator LoadSceneSafely(int index)
    {
        // Opcional: Limpieza previa si lo necesitas
        yield return null; // Esperar un frame

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Esperar a que la escena termine de cargar
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Aqu� puedes agregar l�gica si necesitas hacer algo justo despu�s de cargar
        Debug.Log("Escena cargada correctamente");
    }
}
