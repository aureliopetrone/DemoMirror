using UnityEngine;

namespace DemoMirror
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobbyDemo networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }
    }
}
