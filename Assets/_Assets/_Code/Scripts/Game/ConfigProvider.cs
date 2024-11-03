using _Assets._Code.Scripts.Game.UIs.Ghost;
using UnityEngine;

namespace _Assets._Code.Scripts.Game
{
    public class ConfigProvider : MonoBehaviour
    {
        [SerializeField] private GhostMobSO[] ghostSlotSO;
        public GhostMobSO[] GhostSlotSOs => ghostSlotSO; 
    }
}