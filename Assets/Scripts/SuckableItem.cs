using UnityEngine;

public class SuckableItem : MonoBehaviour
{
    [Header("Tipo de objeto")]
    public SuckableType itemType = SuckableType.None;
    
    [Header("Propiedades")]
    public float itemWeight = 1f;
    public int itemValue = 10;
    
    [Header("Referencias")]
    public MeshFilter meshHolder; // Lugar donde instanciar el modelo
    public Mesh[] canVariants;
    public Mesh[] bottleVariants;
    public Mesh[] circuitVariants;
    public Mesh[] organicVariants;
    
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = itemWeight;
        meshHolder = GetComponentInChildren<MeshFilter>();
        SpawnMesh();
    }
    
    void SpawnMesh()
    {
        Mesh[] selectedSet = null;
        itemType = (SuckableType)Random.Range(0, System.Enum.GetValues(typeof(SuckableType)).Length);
        int variantIndex = Random.Range(0, 5);

        switch (itemType)
        {
            case SuckableType.Can: selectedSet = canVariants; break;
            case SuckableType.Bottle: selectedSet = bottleVariants; break;
            case SuckableType.Circuit: selectedSet = circuitVariants; break;
            case SuckableType.Organic: selectedSet = organicVariants; break;
        }

        if (selectedSet != null && variantIndex >= 0 && variantIndex < selectedSet.Length)
        {
            meshHolder.mesh = selectedSet[variantIndex];
        }
        else
        {
            Debug.LogWarning("Variante inválida para " + itemType);
        }
    }
}
