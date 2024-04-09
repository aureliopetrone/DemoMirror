using DemoMirror;
using Mirror;
using System.Linq;
using UnityEngine;

namespace DemoMirror
{
    public class RoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;

        private NetworkManagerLobbyDemo room;
        private NetworkManagerLobbyDemo Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobbyDemo;
            }
        }

        public void CountdownEnded()
        {
            animator.enabled = false;
        }

        #region Server

        public override void OnStartServer()
        {
            NetworkManagerLobbyDemo.OnServerStopped += CleanUpServer;
            NetworkManagerLobbyDemo.OnServerReadied += CheckToStartRound;
        }

        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            NetworkManagerLobbyDemo.OnServerStopped -= CleanUpServer;
            NetworkManagerLobbyDemo.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        [Server]
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }

            animator.enabled = true;

            RpcStartCountdown();
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcStartCountdown()
        {
            animator.enabled = true;
        }

        [ClientRpc]
        private void RpcStartRound()
        {
            Debug.Log("Start Round");
            InputManager.Remove(ActionMapNames.Player);
        }

        #endregion
    }
}