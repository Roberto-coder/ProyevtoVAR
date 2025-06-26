using UnityEngine;

public class SuckableItem : MonoBehaviour
{
    [Header("Tipo de objeto")]
    public SuckableType itemType = SuckableType.None;
    
    [Header("Propiedades")]
    public float itemWeight = 1f;
    public int itemValue = 10;
    
    [Header("Referencias")]
    private GameObject currentInstance;
    public GameObject[] canVariants;
    public GameObject[] bottleVariants;
    public GameObject[] circuitVariants;
    public GameObject[] organicVariants;
    
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = itemWeight;
        SpawnMesh();
    }
    
    void SpawnMesh()
    {
        GameObject[] selectedSet = null;
        itemType = (SuckableType)Random.Range(0, System.Enum.GetValues(typeof(SuckableType)).Length);
        int variantIndex = Random.Range(0, 5);

        switch (itemType)
        {
            case SuckableType.Can: selectedSet = canVariants; break;
            case SuckableType.Bottle: selectedSet = bottleVariants; break;
            case SuckableType.Circuit: selectedSet = circuitVariants; break;
            case SuckableType.Organic: selectedSet = organicVariants; break;
        }
        
        if (currentInstance != null)
            Destroy(currentInstance);

        if (selectedSet != null && variantIndex >= 0 && variantIndex < selectedSet.Length)
        {
            currentInstance = Instantiate(selectedSet[variantIndex], transform);
        }
        else
        {
            Debug.LogWarning("Variante inválida para " + itemType);
        }
    }
}
