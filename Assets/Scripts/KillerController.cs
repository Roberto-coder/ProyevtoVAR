using UnityEngine;

public class KillerController : MonoBehaviour
{
    public GameObject player;
    public TimerController watcherController;
    public float penalty = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Resetea la posición del jugador a -125, -4, 100
            Vector3 respawnPosition = new Vector3(-125f, -4f, 100f);
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = respawnPosition;
            }
            else
            {
                player.transform.position = respawnPosition;
            }
            // Aquí puedes agregar la lógica que deseas ejecutar al entrar en el trigger
            Debug.Log("El jugador ha entrado en el trigger del KillerController.");
            watcherController.tiempoRestante -= penalty;
        }
    }
}
