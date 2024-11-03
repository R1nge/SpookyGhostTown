using System;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.Ghosts
{
    public class GhostMobPlacer : NetworkBehaviour
    {
        [SerializeField] private NetworkObject mob;
        [SerializeField] private Transform camera;
        private GhostMobType _ghostMobType;
        public event Action<GhostMobType> OnMobTypeChanged;

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            ChangeType();

            if (Input.GetMouseButtonDown(0))
            {
                Raycast();
            }
        }

        private void ChangeType()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _ghostMobType = GhostMobType.SimpleMob;
                OnMobTypeChanged?.Invoke(_ghostMobType);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            }
        }

        private void Raycast()
        {
            var ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit))
            {
                SpawnMobServerRpc(hit.point + Vector3.up, _ghostMobType);
            }
        }

        [Rpc(SendTo.Server)]
        private void SpawnMobServerRpc(Vector3 position, GhostMobType ghostMobType)
        {
            SpawnMob(position, ghostMobType);
        }

        private void SpawnMob(Vector3 position, GhostMobType ghostMobType)
        {
            switch (ghostMobType)
            {
                case GhostMobType.None:
                    break;
                case GhostMobType.SimpleMob:
                    Instantiate(mob, position, Quaternion.identity).Spawn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ghostMobType), ghostMobType, null);
            }
        }

        public enum GhostMobType : byte
        {
            None = 0,
            SimpleMob = 1
        }
    }
}