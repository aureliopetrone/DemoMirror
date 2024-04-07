using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DemoMirror
{
    public class JoinMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobbyDemo networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;
        [SerializeField] private TMP_InputField ipAddressInputField = null;
        [SerializeField] private Button joinButton = null;

        private void OnEnable()
        {
            NetworkManagerLobbyDemo.OnClientConnected += HandleClientConnected;
            NetworkManagerLobbyDemo.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobbyDemo.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobbyDemo.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;

            // If ipAddress is empty, set it to default localhost
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = "localhost";
            }

            networkManager.networkAddress = ipAddress;
    

            networkManager.StartClient();

            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}
