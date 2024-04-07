using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DemoMirror;
using Mirror;

public class LobbyPlayerSlotUI : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI displayNameText = null;
    [SerializeField] private TextMeshProUGUI readyUpText = null;
    [SerializeField] private Button readyUpButton = null;


    // Set button handler on load
    private void Start()
    {
        readyUpButton.interactable = false;
    }



    [Command]
    public void CmdSetPlayerInfo(string displayName, bool isReady)
    {
        displayNameText.text = displayName;
        readyUpText.text = isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
    }

    override public void OnStartAuthority()
    {
        

        // If the player is the owner, enable the ready up button
        if (isOwned){
            readyUpButton.interactable = true;        
        }

        // Set the button handler
        readyUpButton.onClick.AddListener(() =>
        {
            // Get the local player
            DemoMirror.NetworkRoomPlayerLobbyDemo roomPlayer = NetworkClient.connection.identity.GetComponent<DemoMirror.NetworkRoomPlayerLobbyDemo>();
            Debug.Log("Local player: " + roomPlayer);

            roomPlayer.CmdReadyUp();
            CmdSetPlayerInfo(roomPlayer.DisplayName, roomPlayer.IsReady);
        });
    }

    public void ResetPlayerInfo()
    {
        displayNameText.text = "Waiting for player...";
        readyUpText.text = "";
    }



}
