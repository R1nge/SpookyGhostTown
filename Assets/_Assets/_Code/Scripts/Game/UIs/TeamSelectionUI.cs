using System.Collections.Generic;
using _Assets._Code.Scripts.Lobby;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets._Code.Scripts.Game.UIs
{
    public class TeamSelectionUI : NetworkBehaviour
    {
        [SerializeField] private Button start;
        [SerializeField] private Button changeTeam;
        [SerializeField] private TeamSelectionPlayerUI teamSelectionPlayerUIPrefab;
        [SerializeField] private Transform playerList;
        [SerializeField] private LobbyService lobbyService;
        [SerializeField] private GameObject ui;
        private readonly Dictionary<ulong, TeamSelectionPlayerUI> _players = new();

        private void Start()
        {
            lobbyService.OnGameStarted += HideUI;
            lobbyService.OnPlayerConnected += UpdateUI;
            lobbyService.OnTeamChanged += OnTeamChanged;
            start.onClick.AddListener(StartGame);
            changeTeam.onClick.AddListener(ChangeTeam);
        }

        private void OnTeamChanged(ulong clientId)
        {
            _players[clientId].UpdateTeam(clientId.ToString(), lobbyService.GetTeamServer(clientId).ToString());
            OnTeamChangedClientRpc(clientId, lobbyService.GetTeamServer(clientId));
        }

        [Rpc(SendTo.Everyone)]
        private void OnTeamChangedClientRpc(ulong clientId, LobbyService.Teams teams)
        {
            _players[clientId].UpdateTeam(clientId.ToString(), teams.ToString());
        }

        private void UpdateUI(ulong clientId)
        {
            var playerUI = Instantiate(teamSelectionPlayerUIPrefab, playerList);
            _players.Add(clientId, playerUI);
            playerUI.UpdateTeam(clientId.ToString(), lobbyService.GetTeamServer(clientId).ToString());
        }

        private void HideUI()
        {
            ui.SetActive(false);
        }

        private void StartGame()
        {
            if (IsServer)
            {
                lobbyService.StartGame();
            }
        }

        private void ChangeTeam()
        {
            lobbyService.ChangeTeamServerRpc(NetworkManager.LocalClientId);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            lobbyService.OnGameStarted -= HideUI;
        }
    }
}