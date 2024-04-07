using Mirror;

namespace DemoMirror
{
    public class NetworkGamePlayerLobbyDemo : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";

        private NetworkManagerLobbyDemo room;
        private NetworkManagerLobbyDemo Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobbyDemo;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
    }
}