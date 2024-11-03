using _Assets._Code.Scripts.Lobby;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.SafeZones
{
    public class SafeZone : NetworkBehaviour
    {
        [SerializeField] private LobbyService LobbyService;
        private readonly NetworkVariable<int> _playerInTheSafeZone = new NetworkVariable<int>();

        private void OnTriggerEnter(Collider other)
        {
            if (IsServer)
            {
                _playerInTheSafeZone.Value++;

                if (_playerInTheSafeZone.Value == LobbyService.NumberOfSurvivors)
                {
                    Debug.Log("SURVIVORS ESCAPED");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsServer)
            {
                _playerInTheSafeZone.Value--;
            }
        }
    }
}