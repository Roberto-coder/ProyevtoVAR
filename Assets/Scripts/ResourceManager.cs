using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public static int Metal { get; private set; }
    public static int Components { get; private set; }
    public static int Score { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public static void AddResources(int metal, int comps)
    {
        Metal += metal;
        Components += comps;
        ResourceManager.UpdateScore(0);

    }

    public static void UpdateScore(int delta)
    {
        Score += delta;
        UIManager.Instance?.RefreshUI();
    }

    public static bool TrySpendResources(int metal, int components)
    {
        if (Metal >= metal && Components >= components)
        {
            Metal -= metal;
            Components -= components;
            UpdateScore(0);
            return true;
        }
        return false;
    }


}


