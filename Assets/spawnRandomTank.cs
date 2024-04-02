using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnRandomTank : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InstantiateRandomTank(new Vector3(0, 0, 0));
    }

    void InstantiateRandomTank(Vector3 position) {

         // Create an instance of the tank prefab
        GameObject tank = Instantiate(Resources.Load("Tank"), position, Quaternion.identity) as GameObject;

        // Assign the tank as a variable to the script on the main camera
        Camera.main.GetComponent<CameraMovement>().tank = tank;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
