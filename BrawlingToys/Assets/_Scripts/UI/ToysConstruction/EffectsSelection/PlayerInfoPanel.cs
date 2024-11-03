using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using BrawlingToys.Actors;
using TMPro;
using Unity.Netcode;
using BrawlingToys.Managers;
using System.Linq;

namespace BrawlingToys.UI
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        private Player _player;
        [SerializeField]
        private TextMeshProUGUI _playerName;
        [SerializeField]
        private RawImage _modelRawImage;
        [SerializeField]
        private Transform _effectsHorizontalLayout;
        [SerializeField]
        private UnityEvent<PlayerInfoPanel> _onClicked;

        public Player Player { get => _player; }

        public void FillInfoPanel( Player player, string playerName, AssetReference characterAsset, GameObject[] effectsGo)
        {
            _player = player;
            _playerName.text = playerName;

            foreach (GameObject go in effectsGo)
            {
                go.transform.SetParent(_effectsHorizontalLayout);
            }

            CreateEffectIcons(_player.Stats);

            ModelSpawner.Instance.SpawnRenderTextureModelWithNewCamera(characterAsset, _modelRawImage);
        }

        private void CreateEffectIcons(Stats playerStats)
        {
            if (playerStats.Mediator.GetAppliedModifiers() == null)
                return;

            foreach (ModifierScriptable mod in playerStats.Mediator.GetAppliedModifiers())
            {
                EffectIconGenerator.instance.CreateEffectIcon(mod, _effectsHorizontalLayout);
            }
        }

        public UnityEvent<PlayerInfoPanel> GetPlayerInfoClickEvent() => _onClicked;

        public void OnPlayerInfoPanelClicked()
        {
            _onClicked?.Invoke(this);
        }
    }
}