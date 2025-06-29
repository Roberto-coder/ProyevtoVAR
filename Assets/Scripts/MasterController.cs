using UnityEngine;

public class MasterController : MonoBehaviour
{
    public Transform gripAnchor;
    public float pickupRange = 0.2f;

    private PickUpGun currentTool;

    void Update()
    {
        if (currentTool != null)
        {
            // Botón B suelta la herramienta
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                currentTool.OnDrop();
                currentTool = null;
            }
        }
        else
        {
            AutoPickup();
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
