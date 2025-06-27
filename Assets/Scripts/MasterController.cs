using UnityEngine;

public class MasterController : MonoBehaviour
{
    public bool enableAutoPickup = true;
    public Transform gripAnchor;
    public float pickupRange = 0.2f;
    public MonoBehaviour currentTool;

    void Update()
    {
        if (currentTool != null && enableAutoPickup)
        {
            // Bot�n B suelta la herramienta
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                ((ToolI)currentTool).OnDrop();
                currentTool = null;
            }
        }
        else
        {
            if (enableAutoPickup)
                AutoPickup();
            else
            {
                ((ToolI)currentTool).OnPickup(gripAnchor);
            }
        }
    }

    void AutoPickup()
    {
        Collider[] hits = Physics.OverlapSphere(gripAnchor.position, pickupRange);
        foreach (var c in hits)
        {
            var tool = c.GetComponent<PickUpGun>();
            if (tool != null && !tool.IsPicked())
            {
                tool.OnPickup(gripAnchor);
                currentTool = tool;
                Debug.Log("Herramienta recogida: " + tool.name);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (gripAnchor)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gripAnchor.position, pickupRange);
        }
    }
}
