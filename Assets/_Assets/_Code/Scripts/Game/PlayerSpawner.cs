using System;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkObject ghostPrefab;
        [SerializeField] private NetworkObject survivorPrefab;
        private NetworkManager _networkManager;

        private void Start()
        {
            _networkManager = NetworkManager.Singleton;
            _networkManager.OnClientConnectedCallback += OnConnection;
        }

        private void OnConnection(ulong clientId)
        {
            if (_networkManager.IsHost)
            {
                SpawnGhost(clientId);
            }
            else
            {
                SpawnSurvivor(clientId);
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