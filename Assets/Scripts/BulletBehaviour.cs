using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DemoMirror;
public class BulletBehaviour : NetworkBehaviour
{
    // Start is called before the first frame update

    

    void Start()
    {

      Debug.Log("Bullet created");

      // Destroy the bullet after 5 seconds
      Destroy(gameObject, 5);

    }

    // Update is called once per frame
      // Questo metodo viene chiamato quando il collider (impostato come Trigger) entra in contatto con un altro collider
    private void OnCollisionEnter(Collision other) {

        Debug.Log("Bullet destroyed");
        

        // Check if the collision is with a player
        if(other.gameObject.tag == "Player") {

            Debug.Log("Player hit");
            // Get the health component of the player
            Health health = other.gameObject.GetComponent<Health>();
            // Deal damage to the player
            health.CmdDealDamage(10);
        }


        //Destroy(gameObject); // Distrugge il proiettile
        
    }

    
}
