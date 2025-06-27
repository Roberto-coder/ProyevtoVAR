using UnityEngine;
using System;

public class TreeResourceManager : MonoBehaviour
{
    public static TreeResourceManager Instance { get; private set; }

    public int TreesDestroyed { get; private set; }
    public int TreeScore { get; private set; }

    public event Action OnScoreChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddTree(int scorePerTree)
    {
        TreesDestroyed++;
        TreeScore += scorePerTree;
        OnScoreChanged?.Invoke();
        Debug.Log($"Árboles: {TreesDestroyed}, Puntos: {TreeScore}");
    }
}
