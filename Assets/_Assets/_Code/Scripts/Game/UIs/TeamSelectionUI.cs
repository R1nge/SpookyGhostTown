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

        private void OnTeamChanged(LobbyService.LobbyPlayerData lobbyPlayerData)
        {
            _players[lobbyPlayerData.ClientId].UpdateTeam(lobbyPlayerData.Name, lobbyPlayerData.Team.ToString());
        }

        private void UpdateUI(LobbyService.LobbyPlayerData lobbyPlayerData)
        {
            if (!IsServer) return;
            var playerUI = Instantiate(teamSelectionPlayerUIPrefab, playerList);
            playerUI.GetComponent<NetworkObject>().Spawn();
            playerUI.transform.SetParent(playerList);
            _players.Add(lobbyPlayerData.ClientId, playerUI);
            playerUI.UpdateTeam(lobbyPlayerData.Name, lobbyPlayerData.Team.ToString());
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
            lobbyService.OnPlayerConnected -= UpdateUI;
            lobbyService.OnTeamChanged -= OnTeamChanged;
        }
    }
}