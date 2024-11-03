using System;
using _Assets._Code.Scripts.Game.Ghosts;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs.Ghost
{
    public class GhostMobSelectionUI : MonoBehaviour
    {
        [SerializeField] private GhostMobSelectionSlotUI[] slots;
        [SerializeField] private GhostMobPlacer ghostMobPlacer;
        private int _selectedSlotIndex = 0;

        private void Start()
        {
            ghostMobPlacer.OnMobTypeChanged += SelectSlot;
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Deselect();
            }
        }

        private void SelectSlot(GhostMobPlacer.GhostMobType mobType)
        {
            switch (mobType)
            {
                case GhostMobPlacer.GhostMobType.None:
                    break;
                case GhostMobPlacer.GhostMobType.SimpleMob:
                    SelectSlot(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mobType), mobType, null);
            }
        }

        public void SelectSlot(int index)
        {
            slots[_selectedSlotIndex].Deselect();
            slots[index].Select();
            _selectedSlotIndex = index;
        }

        private void OnDestroy()
        {
            ghostMobPlacer.OnMobTypeChanged -= SelectSlot;
        }
    }
}