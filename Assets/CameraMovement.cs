using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to fix the camera to the tank

public class CameraMovement : MonoBehaviour
{

    public GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the tank position
        UnityEngine.Vector3 tankPosition = tank.transform.position;
        // Get the tank rotation
        UnityEngine.Quaternion tankRotation = tank.transform.rotation;

        // Get the position of the camera

        UnityEngine.Vector3 cameraPosition = tankPosition - tankRotation * new UnityEngine.Vector3(0, -5, 10);
    
        // Set the position of the camera
        transform.position = cameraPosition;

        // Look at the tank
        transform.LookAt(tankPosition);

        // Rotate the camera up to look forward

        transform.Rotate(new UnityEngine.Vector3(-30, 0, 0));
        
    }
}
