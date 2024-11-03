using System;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.Ghosts
{
    public class GhostMobPlacer : NetworkBehaviour
    {
        [SerializeField] private NetworkObject mob;
        [SerializeField] private Transform camera;

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Raycast();
            }
        }

        private void Raycast()
        {
            var ray = new Ray(camera.position, camera.forward);
            if (Physics.Raycast(ray, out var hit))
            {
                SpawnMobServerRpc(hit.point + Vector3.up);
            }
        }

        [Rpc(SendTo.Server)]
        private void SpawnMobServerRpc(Vector3 position)
        {
            SpawnMob(position);
        }
        
        private void SpawnMob(Vector3 position)
        {
            Instantiate(mob, position, Quaternion.identity).Spawn();
        }
    }
}