using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vacuum : MonoBehaviour, ToolI
{
    public string ToolName => "Vacuum";
    
    [Header("Zona de Succión y Captura")]
    public Collider suctionZone; // asignar el MeshCollider
    public Collider destroyZone; // asignar el BoxCollider
    
    [Header("Configuración de Succión")]
    public float suctionForce = 50f;
    public Transform suctionPoint;

    public bool isSucking = false;
    
    void Start()
    {
        // Asegurar que los colliders están en modo trigger
        suctionZone.isTrigger = true;
        destroyZone.isTrigger = true;
    }
    
    void Update()
    {
        if (isSucking)
        {
            Collider[] hits = Physics.OverlapBox(
                suctionZone.bounds.center,
                suctionZone.bounds.extents,
                suctionZone.transform.rotation
            );

            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Suckable")) continue;
                SuckableItem item = hit.GetComponent<SuckableItem>();
                if (item == null || !item.gameObject.activeInHierarchy) continue;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb)
                {
                    Vector3 dir = (suctionPoint.position - rb.position).normalized;
                    rb.AddForce(dir * (suctionForce), ForceMode.Force);
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.tag);
    }
    
    // Activar succión (ej. mantener clic izquierdo)
    public void use()
    {
        Debug.Log("Usando la aspiradora.");
        isSucking = true;
    }
    
    // Desactivar succión (ej. soltar clic izquierdo)
    public void stopUse()
    {
        Debug.Log("Dejando de usar la aspiradora.");
        isSucking = false;
    }
}
