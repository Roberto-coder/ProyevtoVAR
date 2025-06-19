using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.Get(OVRInput.Button.Two))
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (PickUpGun.pickedUp == true)
            {
                PickUpGun.pickedUp = false;
            }
        }
    }
}