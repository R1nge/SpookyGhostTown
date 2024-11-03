using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs.TeamSelection
{
    public class TeamSelectionPlayerUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI teamText;
        private readonly NetworkVariable<FixedString128Bytes> _nickname = new();

        private void Start()
        {
            _nickname.OnValueChanged += UpdateUI;
            UpdateUI(_nickname.Value, _nickname.Value);
        }

        public void UpdateTeam(FixedString128Bytes nick, string team) => _nickname.Value = $"{nick} - {team}";

        private void UpdateUI(FixedString128Bytes _, FixedString128Bytes str) => teamText.text = str.Value;

        public override void OnDestroy()
        {
            base.OnDestroy();
            _nickname.OnValueChanged -= UpdateUI;
        }
    }
}