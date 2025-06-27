using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f); // Eje Y por defecto

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
