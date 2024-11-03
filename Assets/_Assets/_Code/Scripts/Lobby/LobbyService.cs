using System;
using System.Collections.Generic;
using System.Linq;
using _Assets._Code.Scripts.Game;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Assets._Code.Scripts.Lobby
{
    public class LobbyService : NetworkBehaviour
    {
        [SerializeField] private PlayerSpawner playerSpawner;
        
        private readonly Dictionary<ulong, Teams> _teams = new();
        public event Action OnGameStarted;
        public event Action<ulong> OnPlayerConnected;
        public event Action<ulong> OnTeamChanged; 

        private void Start()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnection;
        }

        private void OnConnection(ulong clientId)
        {
            _teams.Add(clientId, Teams.None);
            Debug.Log("Add client to the NONE team");
            OnPlayerConnected?.Invoke(clientId);
        }

        [Rpc(SendTo.Server)]
        public void SetTeamServerRpc(ulong clientId, Teams team)
        {
            _teams[clientId] = team;
            OnTeamChanged?.Invoke(clientId);
        }

        [Rpc(SendTo.Server)]
        public void ChangeTeamServerRpc(ulong clientId)
        {
            if (_teams[clientId] == Teams.Ghosts)
            {
                SetTeamServerRpc(clientId, Teams.Survivors);
            }
            else if (_teams[clientId] == Teams.Survivors)
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
            return _teams[clientId];
        }

        public void StartGame()
        {
            StartGameEveryoneRpc();
        }

        [Rpc(SendTo.Everyone)]
        private void StartGameEveryoneRpc()
        {
            OnGameStarted?.Invoke();
            for (int i = 0; i < _teams.Count; i++)
            {
                playerSpawner.SpawnPlayersServerRpc(_teams.Keys.ToArray()[i], _teams.Values.ToArray()[i]);
            }
        }

        public enum Teams : byte
        {
            None = 0,
            Survivors = 1,
            Ghosts = 2
        }
    }
}