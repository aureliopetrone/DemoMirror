using DemoMirror;
using Mirror;
using System.Linq;
using UnityEngine;
using TMPro;


namespace DemoMirror
{
    public class Health : NetworkBehaviour
    {
        [SyncVar(hook = nameof(HandleHealthUpdated))]
        [SerializeField] private int health = 100;

        [SerializeField] private TMP_Text healthText = null;

        public event System.Action ServerOnDie;

        private bool isDead = false;

        public int GetHealth()
        {
            return health;
        }

        public bool IsDead()
        {
            return isDead;
        }

        [Command]
        public void CmdDealDamage(int damage)
        {
            Debug.Log("CmdDealDamage");
            if (isDead) { return; }

            health = Mathf.Max(health - damage, 0);

            if (health != 0) { return; }

            isDead = true;

            ServerOnDie?.Invoke();
        }

        private void HandleHealthUpdated(int oldHealth, int newHealth)
        {
            // update UI or whatever
            Debug.Log("HandleHealthUpdated");
            healthText.text = newHealth.ToString();
        }

        public override void OnStartAuthority()
        {
            if(!isOwned) { return; }
            
            healthText.enabled = true;
            HandleHealthUpdated(0, health);
        }
    }

}