using UnityEngine;
using System.Collections;

public class GreenTreeScaler : MonoBehaviour
{
    public float growDuration = 1.5f;
    private Vector3 targetScale;

    void Start()
    {
        targetScale = Vector3.one;
        StartCoroutine(ScaleTree());
    }

    IEnumerator ScaleTree()
    {
        float t = 0f;
        while (t < growDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t / growDuration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(this); // ya no se necesita este script
    }
}
