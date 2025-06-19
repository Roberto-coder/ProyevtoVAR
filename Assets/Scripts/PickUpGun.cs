using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    GameObject startTrans;
    Transform controller;
    public static bool pickedUp = false;

    void Start()
    {
        startTrans = new GameObject();
        startTrans.transform.position = transform.position;
        startTrans.transform.rotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (pickedUp && controller != null)
        {
            transform.position = controller.position;
            transform.rotation = controller.rotation;
        }
        else
        {
            transform.position = startTrans.transform.position;
            transform.rotation = startTrans.transform.rotation;
            transform.SetParent(null);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        pickedUp = true;
        controller = other.transform;
        transform.SetParent(controller);
    }
}
