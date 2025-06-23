using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    public Vacuum vacuum; // Asigna esto en el inspector
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suckable"))
        {
            Debug.Log("Objeto entró en la zona de destrucción: " + other.name);
            SuckableItem item = other.GetComponent<SuckableItem>();
            Debug.Log("Estado de succión: " + vacuum.isSucking);
            if (item != null && vacuum.isSucking)
            {
                Debug.Log("Destruyendo objeto: " + item.itemType);
                Destroy(item.gameObject);
            }
        }
    }
}