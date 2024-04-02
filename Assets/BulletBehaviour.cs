using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    

    void Start()
    {

      Debug.Log("Bullet created");

    }

    // Update is called once per frame
      // Questo metodo viene chiamato quando il collider (impostato come Trigger) entra in contatto con un altro collider
    private void OnCollisionEnter(Collision other) {

        Debug.Log("Bullet destroyed");
        Destroy(gameObject); // Distrugge il proiettile
    }
}
