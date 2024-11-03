using System;
using System.Collections.Generic;
using System.Linq;
using _Assets._Code.Scripts.Game;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Assets._Code.Scripts.Lobby
{
    public class LobbyService : NetworkBehaviour
    {
        [SerializeField] private PlayerSpawner playerSpawner;

        public int NumberOfPlayers => _playerData.Keys.Count;
        public int NumberOfSurvivors => _playerData.Values.Count(playerData => playerData.Team == Teams.Survivors);
        private readonly Dictionary<ulong, LobbyPlayerData> _playerData = new();
        public event Action OnGameStarted;
        public event Action<LobbyPlayerData> OnPlayerConnected;
        public event Action<LobbyPlayerData> OnTeamChanged; 

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnection;
        }

        private void OnConnection(ulong clientId)
        {
            var playerData = new LobbyPlayerData
            {
                ClientId = clientId,
                Name = $"Player {clientId}",
                IsAlive = true,
                Team = Teams.None
            };
            
            _playerData.Add(clientId, playerData);
            Debug.Log("Add client to the NONE team");
            OnPlayerConnected?.Invoke(playerData);
        }

        [Rpc(SendTo.Server)]
        public void SetTeamServerRpc(ulong clientId, Teams team)
        {
            var lobbyPlayerData = _playerData[clientId];
            lobbyPlayerData.Team = team;
            _playerData[clientId] = lobbyPlayerData;
            OnTeamChanged?.Invoke(lobbyPlayerData);
        }

        [Rpc(SendTo.Server)]
        public void ChangeTeamServerRpc(ulong clientId)
        {
            if (_playerData[clientId].Team == Teams.Ghosts)
            {
                SetTeamServerRpc(clientId, Teams.Survivors);
            }
            else if (_playerData[clientId].Team == Teams.Survivors)
            {
                SetTeamServerRpc(clientId, Teams.Ghosts);
            }
            else
            {
                var randomTeam = Random.Range(0, 2) == 0 ? Teams.Ghosts : Teams.Survivors;
                SetTeamServerRpc(clientId, randomTeam);
            }
        }

        public Teams GetTeamServer(ulong clientId)
        {
            return _playerData[clientId].Team;
        }

        public void StartGame()
        {
            StartGameEveryoneRpc();
        }

        [Rpc(SendTo.Everyone)]
        private void StartGameEveryoneRpc()
        {
            OnGameStarted?.Invoke();
            for (int i = 0; i < _playerData.Count; i++)
            {
                playerSpawner.SpawnPlayersServerRpc(_playerData.Keys.ToArray()[i], _playerData.Values.ToArray()[i].Team);
            }
        }

        public enum Teams : byte
        {
            None = 0,
            Survivors = 1,
            Ghosts = 2
        }
        
        [Serializable]
        public struct LobbyPlayerData
        {
            public ulong ClientId;
            public FixedString128Bytes Name;
            public Teams Team;
            public bool IsAlive;
        }
    }
}