using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.HealthSystem
{
    public abstract class HealthBase : NetworkBehaviour, IDamageable
    {
        [SerializeField] private NetworkVariable<int> health;

        public NetworkVariable<int> Health => health;

        public void TakeDamage(int damage) => TakeDamageProtected(damage);

        protected abstract void TakeDamageProtected(int damage);
    }
}