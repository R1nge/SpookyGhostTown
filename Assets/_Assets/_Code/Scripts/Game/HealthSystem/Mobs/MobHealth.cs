using UnityEngine;

namespace _Assets._Code.Scripts.Game.HealthSystem.Mobs
{
    public class MobHealth : HealthBase
    {
        protected override void TakeDamageProtected(int damage)
        {
            Debug.Log($"Mob took damage {damage}");
            Destroy(gameObject);
        }
    }
}