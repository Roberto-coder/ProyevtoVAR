using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vacuum : MonoBehaviour, ToolI
{
    public string ToolName => "Vacuum";
    public Transform controller { get; set; }
    public bool isPicked { get; set; }

    [Header("Zona de Succión y Captura")]
    public Collider suctionZone; // asignar el MeshCollider
    public Collider destroyZone; // asignar el BoxCollider
    
    [Header("Configuración de Succión")]
    public float suctionForce = 50f;
    public Transform suctionPoint;

    public bool isSucking = false;
    
    [Header("Sonidos")]
    public AudioClip suctionStartClip;
    public AudioClip suctionStopClip;

    private AudioSource audioSource;
    private Coroutine fadeOutCoroutine;
    private float defaultVolume = 1f;
    public float fadeDuration = 0.8f;

    void Start()
    {
        // Asegurar que los colliders están en modo trigger
        suctionZone.isTrigger = true;
        destroyZone.isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        defaultVolume = audioSource.volume;
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
    public void use(Collider other = null)
    {
        Debug.Log("Usando la aspiradora.");
        isSucking = true;
        if (audioSource && suctionStartClip)
        {
            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine);
            audioSource.volume = defaultVolume;
            audioSource.clip = suctionStartClip;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    public void stopUse()
    {
        Debug.Log("Dejando de usar la aspiradora.");
        isSucking = false;
        if (audioSource && suctionStartClip)
        {
            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = StartCoroutine(FadeOutAndStop());
        }
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = defaultVolume;
    }
    
    public void OnPickup(Transform parent)
    {
        isPicked = true;
    }

    public void OnDrop()
    { }

    public bool IsPicked()
    {
        return true;
    }
}
