using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    GameObject startTrans;
    Transform controller;
    public static bool pickedUp = false;

    public string targetTag = "Destructible";
    public int maxHits = 3;
    int hitCount = 0;

    void Start()
    {
        startTrans = new GameObject("StartPosition");
        startTrans.transform.position = transform.position;
        startTrans.transform.rotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (pickedUp && controller != null)
        {
            transform.position = controller.position;
            transform.rotation = controller.rotation;
        }
        else
        {
            transform.SetParent(null);
            transform.position = startTrans.transform.position;
            transform.rotation = startTrans.transform.rotation;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!pickedUp)
        {
            pickedUp = true;
            controller = other.transform;
            transform.SetParent(controller);
        }

        // Registro de impactos en objetos específicos
        if (other.CompareTag(targetTag))
        {
            hitCount++;
            if (hitCount >= maxHits)
                Destroy(other.gameObject);
        }
    }
}


