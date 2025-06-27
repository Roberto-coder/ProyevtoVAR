using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject[] toolObjects; // GameObjects que contienen los scripts
    private ToolI[] tools;
    public Transform controller;
    private int currentToolIndex = 0;

    private float inputCooldown = 0.5f;
    private float lastSwitchTime = 0f;

    void Start()
    {
        tools = new ToolI[toolObjects.Length];
        for (int i = 0; i < toolObjects.Length; i++)
        {
            tools[i] = toolObjects[i].GetComponent<ToolI>();
            toolObjects[i].SetActive(i == currentToolIndex);
        }

        transform.SetParent(controller);
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        // Comenzar a usar herramienta con Botón X (OVRInput.Button.Two)
        if (OVRInput.GetDown(OVRInput.Button.Two))
            tools[currentToolIndex].use();

        // Detener herramienta al soltar Botón X
        if (OVRInput.GetUp(OVRInput.Button.Two))
            tools[currentToolIndex].stopUse();

        // Cambiar herramienta con Joystick derecho arriba/abajo
        float scroll = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;

        if (Time.time - lastSwitchTime > inputCooldown)
        {
            if (scroll > 0.5f)
            {
                int nextIndex = (currentToolIndex + 1) % tools.Length;
                SwitchTool(nextIndex);
                lastSwitchTime = Time.time;
            }
            else if (scroll < -0.5f)
            {
                int prevIndex = (currentToolIndex - 1 + tools.Length) % tools.Length;
                SwitchTool(prevIndex);
                lastSwitchTime = Time.time;
            }
        }
    }

    void SwitchTool(int index)
    {
        if (index == currentToolIndex) return;

        toolObjects[currentToolIndex].SetActive(false);
        toolObjects[index].SetActive(true);
        currentToolIndex = index;

        Debug.Log("Herramienta activa: " + tools[currentToolIndex].ToolName);
    }
}
