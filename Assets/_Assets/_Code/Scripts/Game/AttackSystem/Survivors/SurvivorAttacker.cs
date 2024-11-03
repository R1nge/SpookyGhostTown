using System;
using _Assets._Code.Scripts.Game.HealthSystem;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.AttackSystem.Survivors
{
    public class SurvivorAttacker : NetworkBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private Camera camera;

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Attack");
                DealDamageServerRpc(damage, camera.transform.position, camera.transform.forward);
            }
        }

        [Rpc(SendTo.Server)]
        private void DealDamageServerRpc(int damage, Vector3 position, Vector3 forward)
        {
            var ray = new Ray(position, forward);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.Log($"Hit {hit.transform.name}");
                hit.transform.root.GetComponent<IDamageable>()?.TakeDamage(damage);
            }
        }
    }
}