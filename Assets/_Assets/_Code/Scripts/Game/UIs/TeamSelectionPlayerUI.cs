﻿using TMPro;
using UnityEngine;

namespace _Assets._Code.Scripts.Game.UIs
{
    public class TeamSelectionPlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI teamText;

        public void UpdateUI(string nick, string team)
        {
            teamText.text = $"{nick} - {team}";
        }
    }
}