using System;
using _Assets._Code.Scripts.Game.HealthSystem;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs
{
    public class SurvivorHealthUI : NetworkBehaviour
    {
        [SerializeField] private GameObject ui;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private HealthBase health;

        private void Start()
        {
           ui.SetActive(IsOwner);
           health.Health.OnValueChanged += SetHealth;
           SetHealth(0, health.Health.Value);
        }

        public void SetHealth(int prevHealth, int health)
        {
            healthText.text = $"HP: {health}";
        }

        private void OnDestroy()
        {
            health.Health.OnValueChanged -= SetHealth;
        }
    }
}