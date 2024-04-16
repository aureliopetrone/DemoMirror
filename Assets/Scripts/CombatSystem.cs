using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CombatSystem : NetworkBehaviour
{
  // Start is called before the first frame update

  public GameObject bulletPrefab;

  // This method is called when the player presses the space bar to instantiate a bullet
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Debug.Log("Fire");
      CmdFire();
    }
  }

  [Command]
  private void CmdFire()
  {
    // Get the position of the player
    
    GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward, Quaternion.identity);
    bullet.GetComponent<Rigidbody>().velocity = transform.forward * 50;
    NetworkServer.Spawn(bullet);
  }

}
