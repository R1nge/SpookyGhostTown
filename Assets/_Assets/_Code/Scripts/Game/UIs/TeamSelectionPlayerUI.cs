﻿using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs
{
    public class TeamSelectionPlayerUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI teamText;
        private readonly NetworkVariable<FixedString64Bytes> _nickname = new();

        private void Start()
        {
            _nickname.OnValueChanged += UpdateUI;
            UpdateUI(_nickname.Value, _nickname.Value);
        }

        public void UpdateTeam(string nick, string team) => _nickname.Value = $"{nick} - {team}";

        private void UpdateUI(FixedString64Bytes _, FixedString64Bytes str) => teamText.text = str.Value;

        public override void OnDestroy()
        {
            base.OnDestroy();
            _nickname.OnValueChanged -= UpdateUI;
        }
    }
}