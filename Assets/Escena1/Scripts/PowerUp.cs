using UnityEngine;

public enum PowerUpType { DoublePoints, SpeedBoost, Rain }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 10f;

    private void Start()
    {
        // Autodestruir si no se recoge en cierto tiempo
        Destroy(gameObject, 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpManager.Instance.ActivatePower(type, duration);
            Destroy(gameObject); // desaparece al tocarlo
        }
    }
}
