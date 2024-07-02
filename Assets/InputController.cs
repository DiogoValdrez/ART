using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public DroneController droneController;

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float updownInput = Input.GetAxis("Updown");
        float yawInput = Input.GetAxis("Yaw");

        droneController.MoveDrone(verticalInput,horizontalInput,updownInput,yawInput);
    }
}