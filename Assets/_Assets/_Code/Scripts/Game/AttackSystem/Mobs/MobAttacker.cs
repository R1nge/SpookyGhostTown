using System.Linq;
using _Assets._Code.Scripts.Game.HealthSystem;
using _Assets._Code.Scripts.Game.HealthSystem.Survivors;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.AttackSystem.Mobs
{
    public class MobAttacker : NetworkBehaviour
    {
        [SerializeField] private float attackRange;
        [SerializeField] private int damage;
        [SerializeField] private float attackCooldown;

        private void Start()
        {
            if (IsServer)
            {
                InvokeRepeating(nameof(Attack), attackCooldown, attackCooldown);
            }
        }

        public void Attack()
        {
            var colliders = Physics.OverlapSphere(transform.position, attackRange);
            var sorted = colliders.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out SurvivorHealth survivorHealth))
                {
                    survivorHealth.TakeDamage(damage);
                    break;
                }
            }
        }
    }
}