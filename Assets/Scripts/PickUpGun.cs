using UnityEngine;

public class PickUpGun : MonoBehaviour, ToolI
{
    public string ToolName => "PickUpGun";
    public ParticleSystem destroyEffect;
    public Transform controller { get; set; }
    public bool isPicked { get; set; }

    [Space]
    public int maxHits = 3;
    public int metalGain = 100;
    public int componentGain = 0;

    GameObject startTrans;
    int hitCount = 0;

    void Start()
    {
        startTrans = new GameObject("StartPosition");
        startTrans.transform.position = transform.position;
        startTrans.transform.rotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (isPicked && controller != null)
        {
            transform.SetParent(controller);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            transform.SetParent(null);
            transform.position = startTrans.transform.position;
            transform.rotation = startTrans.transform.rotation;
        }
    }

    public bool IsPicked() => isPicked;

    public void use(Collider other)
    {
        hitCount++;
        if (hitCount >= maxHits)
        {
            ResourceManager.AddResources(metalGain, componentGain);
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, other.transform.position, Quaternion.identity);
                destroyEffect.Play(); // Opcional si ya es standalone
            }
            Destroy(other.gameObject);
            hitCount = 0;
        }
    }

    public void stopUse()
    {
    }

    public void OnPickup(Transform parent)
    {
        isPicked = true;
        controller = parent;
        hitCount = 0;
    }

    public void OnDrop()
    {
        isPicked = false;
        controller = null;
        transform.position = startTrans.transform.position;
        transform.rotation = startTrans.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPicked && (other.CompareTag("metal") || other.CompareTag("electronico")))
        {
            use(other);
        }
    }
}
