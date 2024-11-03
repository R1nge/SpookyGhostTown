using UnityEngine;

namespace _Assets._Code.Scripts.Game.HealthSystem.Survivors
{
    public class SurvivorHealth : HealthBase
    {
        protected override void TakeDamageProtected(int damage)
        {
            Health.Value -= damage;
            Debug.Log($"Survivor took damage {damage}"); 
        }
    }
}