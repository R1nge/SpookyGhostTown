using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs.Ghost
{
    [CreateAssetMenu(fileName = "Ghost Mob", menuName = "Configs/Ghost Mob")]
    public class GhostMobSO : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;
        [SerializeField] private string name;
        public string Name => name;
        [SerializeField] private NetworkObject mob;
        public NetworkObject Mob => mob;
    }
}