using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public GameObject[] toolObjects; // GameObjects que contienen los scripts
    private ToolI[] tools;
    private int currentToolIndex = 0;

    void Start()
    {
        tools = new ToolI[toolObjects.Length];
        for (int i = 0; i < toolObjects.Length; i++)
        {
            tools[i] = toolObjects[i].GetComponent<ToolI>();
            toolObjects[i].SetActive(i == currentToolIndex);
        }
    }

    void Update()
    {
        // Comienza a usar
        if (Input.GetButtonDown("Fire1"))
            tools[currentToolIndex].use();
        
        // Termina de usar
        if(Input.GetButtonUp("Fire1"))
        {
            tools[currentToolIndex].stopUse(); // Aquí podrías definir un método para detener la acción si es necesario
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            int direction = scroll > 0 ? 1 : -1;
            int nextIndex = (currentToolIndex + direction + tools.Length) % tools.Length;
            this.SwitchTool(nextIndex);
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
