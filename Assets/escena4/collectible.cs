using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger con: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Recolectado!");
            GameManager.Instance.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
