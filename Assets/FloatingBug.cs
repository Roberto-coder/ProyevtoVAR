using UnityEngine;

public class FloatingBug : MonoBehaviour
{
    public float speed = 1f;
    public float radius = 1f;
    private Vector3 startPos;
    private float offset;

    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed + offset) * 0.5f;
        float x = Mathf.Sin((Time.time + offset) * speed) * radius;
        float z = Mathf.Cos((Time.time + offset) * speed) * radius;
        transform.position = startPos + new Vector3(x, y, z);
    }
}
