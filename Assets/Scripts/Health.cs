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
            if (isDead) { return; }

            health = Mathf.Max(health - damage, 0);

            if (health != 0) { return; }

            isDead = true;

            ServerOnDie?.Invoke();
        }

        private void HandleHealthUpdated(int oldHealth, int newHealth)
        {
            // update UI or whatever
            healthText.text = newHealth.ToString();
        }

        private void Update()
        {
            if (!isOwned) { return; }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdDealDamage(10);
            }
        }

        public override void OnStartAuthority()
        {
            if(!isOwned) { return; }
            
            healthText.enabled = true;
            HandleHealthUpdated(0, health);
        }
    }

}