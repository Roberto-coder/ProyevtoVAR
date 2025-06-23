using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    public Image crosshairImage;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Suckable"))
            {
                crosshairImage.color = Color.red;
            }
            else
            {
                crosshairImage.color = Color.white;
            }
        }
        else
        {
            crosshairImage.color = Color.white;
        }
    }
}