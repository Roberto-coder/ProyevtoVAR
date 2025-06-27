

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // Solo para XRIT

public class SceneLoader : MonoBehaviour
{
    [System.Obsolete]
    public void LoadScene()
    {
        // Buena pr�ctica: Deshabilitar la interacci�n durante la carga
        var interactors = FindObjectsOfType<XRBaseInteractor>();
        foreach (var interactor in interactors)
        {
            interactor.allowHover = false;
            interactor.allowSelect = false;
        }

        SceneManager.LoadScene("Escena1");
    }
}