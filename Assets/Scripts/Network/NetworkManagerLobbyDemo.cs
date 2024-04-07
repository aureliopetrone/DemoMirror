﻿using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemoMirror
{
    public class NetworkManagerLobbyDemo : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene][SerializeField] private string menuScene = string.Empty;
        [Scene][SerializeField] private string gameScene = string.Empty;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobbyDemo roomPlayerPrefab = null;
        [Header("Game")]
        [SerializeField] private NetworkGamePlayerLobbyDemo gamePlayerPrefab = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<NetworkRoomPlayerLobbyDemo> RoomPlayers { get; } = new List<NetworkRoomPlayerLobbyDemo>();
        public List<NetworkGamePlayerLobbyDemo> GamePlayers { get; } = new List<NetworkGamePlayerLobbyDemo>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayerLobbyDemo roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobbyDemo>();
                RoomPlayers.Remove(player);
                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
            GamePlayers.Clear();
            OnServerStopped?.Invoke();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) { return false; }

            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }

            return true;
        }

        public void StartGame()
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if (!IsReadyToStart()) { return; }

                ServerChangeScene(gameScene);
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Assets/Scenes/"))
            {
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                    NetworkServer.Destroy(conn.identity.gameObject);

                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Assets/Scenes/"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "PlayerSpawnSystem"));
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }
    }
}