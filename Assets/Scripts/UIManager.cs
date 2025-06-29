using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI metalText;
    public TextMeshProUGUI componentText;
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
        RefreshUI();
    }

    public void RefreshUI()
    {
        scoreText.text = $"SCORE: {ResourceManager.Score}";
        metalText.text = $"Metal: {ResourceManager.Metal}";
        componentText.text = $"Componentes: {ResourceManager.Components}";
    }
}
