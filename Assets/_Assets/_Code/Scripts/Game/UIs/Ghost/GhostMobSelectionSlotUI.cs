using UnityEngine;
using UnityEngine.UI;

namespace _Assets._Code.Scripts.Game.UIs.Ghost
{
    public class GhostMobSelectionSlotUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image selected;
        [SerializeField] private int index;
        private ConfigProvider _configProvider;

        private void Awake()
        {
            _configProvider = FindObjectsByType<ConfigProvider>(FindObjectsSortMode.None)[0];
            Init(index);
        }

        public void Init(int index)
        {
            if (index >= _configProvider.GhostSlotSOs.Length)
            {
                return;
            }
            image.sprite = _configProvider.GhostSlotSOs[index].Sprite;
        }

        public void Select() => selected.gameObject.SetActive(true);

        public void Deselect() => selected.gameObject.SetActive(false);
    }
}