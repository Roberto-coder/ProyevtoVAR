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
    
    // 🧠 Punto 5: buffer para OverlapBoxNonAlloc
    private Collider[] overlapBuffer = new Collider[20];

    void Start()
    {
        // Asegurar que los colliders están en modo trigger
        suctionZone.isTrigger = true;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        defaultVolume = audioSource.volume;
    }
    
    void OnDisable()
    {
        Debug.LogWarning("Vacuum DESACTIVADO.");
    }
    void OnEnable()
    {
        Debug.Log("Vacuum ACTIVADO.");
    }
    
    // 🧠 Punto 3: Visualización de la zona de succión
    void OnDrawGizmos()
    {
        if (suctionZone != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = suctionZone.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, suctionZone.bounds.size);
        }
    }

    void Update()
    {
        if (isSucking)
        {
            int hitCount = Physics.OverlapBoxNonAlloc(
                suctionZone.bounds.center,
                suctionZone.bounds.extents,
                overlapBuffer,
                suctionZone.transform.rotation
            );

            bool soloYo = true;
            bool alMenosUnoValido = false;

            for (int i = 0; i < hitCount; i++)
            {
                Collider hit = overlapBuffer[i];

                if (hit == suctionZone)
                {
                    Debug.Log("Detectado el propio collider de la aspiradora.");
                    continue;
                }

                soloYo = false;

                Debug.Log($"Detectado: {hit.name} | Tag: {hit.tag} | Activo: {hit.gameObject.activeSelf}");

                if (!hit.CompareTag("Suckable")) continue;

                SuckableItem item = hit.GetComponent<SuckableItem>();
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (item == null || rb == null || !item.gameObject.activeInHierarchy)
                {
                    Debug.LogWarning($"Objeto {hit.name} tiene componentes faltantes o está inactivo.");
                    continue;
                }

                alMenosUnoValido = true;

                Vector3 dir = (suctionPoint.position - rb.position).normalized;
                rb.AddForce(dir * suctionForce, ForceMode.Force);
            }

            // 🚨 Diagnóstico post-evaluación
            if (soloYo)
            {
                Debug.LogWarning("⚠️ Sólo el propio collider fue detectado. Puede que no haya más objetos válidos.");
            }

            if (!alMenosUnoValido && hitCount > 0)
            {
                Debug.LogWarning("⚠️ Se detectaron objetos, pero ninguno es válido para succión. Verifica sus tags, colliders y estado activo.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objeto en zona de aspirado: " + other.tag);
        
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