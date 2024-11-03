using UnityEngine;

namespace _Assets._Code.Scripts.Game.HealthSystem.Survivors
{
    public class SurvivorHealth : HealthBase
    {
        protected override void TakeDamageProtected(int damage)
        {
            Debug.Log($"Survivor took damage {damage}"); 
        }
    }
}