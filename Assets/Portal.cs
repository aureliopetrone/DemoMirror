using System.Collections;
using System.Collections.Generic;
using DemoMirror;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Update is called once per frame

    private NetworkManagerLobbyDemo networkManager
     {
        get
        {


            return NetworkManagerLobbyDemo.singleton as NetworkManagerLobbyDemo;
        }
    }


    void Update()
    {
        // On key "P" pressed, move the player to the other scene
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key was pressed");
            networkManager.LoadPortalScene();

        }
    }
}
