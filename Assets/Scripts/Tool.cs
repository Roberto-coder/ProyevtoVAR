using UnityEngine;

public interface ToolI
{
    string ToolName { get; }
    void use();
    void stopUse();
}
