using TMPro;
using UnityEngine;

public class PromptUIController : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI promptText;

    void Awake()
    {
        if (canvas) canvas.enabled = false;
    }

    public void Show(string msg)
    {
        if (canvas)
        {
            promptText.text = msg;
            canvas.enabled = true;
        }
    }

    public void Hide()
    {
        if (canvas) canvas.enabled = false;
    }
}
