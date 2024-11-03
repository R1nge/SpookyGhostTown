using System;
using _Assets._Code.Scripts.Lobby;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkObject ghostPrefab;
        [SerializeField] private NetworkObject survivorPrefab;

        [Rpc(SendTo.Server)]
        public void SpawnPlayersServerRpc(ulong clientId, LobbyService.Teams teams)
        {
            switch (teams)
            {
                case LobbyService.Teams.None:
                    break;
                case LobbyService.Teams.Survivors:
                    SpawnSurvivor(clientId);
                    break;
                case LobbyService.Teams.Ghosts:
                    SpawnGhost(clientId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(teams), teams, null);
            }
        }

        public void SpawnGhost(ulong ownerId)
        {
            Instantiate(ghostPrefab, transform.position, Quaternion.identity).SpawnWithOwnership(ownerId);
        }

        public void SpawnSurvivor(ulong ownerId)
        {
            Instantiate(survivorPrefab, transform.position, Quaternion.identity).SpawnWithOwnership(ownerId);
        }
    }
}