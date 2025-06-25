using UnityEngine;

public interface ToolI
{
    string ToolName { get; }
    Transform controller { get; set; }
    bool isPicked { get; set; }
    void use(Collider other = null);
    void stopUse();

    public void OnPickup(Transform parent);
    public void OnDrop();
    public bool IsPicked();
}
