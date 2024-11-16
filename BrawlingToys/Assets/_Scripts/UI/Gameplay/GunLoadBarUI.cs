using UnityEngine;
using BrawlingToys.Actors;
using UnityEngine.UI;
using Unity.Netcode;

namespace BrawlingToys.UI
{
    public class GunLoadBarUI : NetworkBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField]
        private RectTransform _cursorTrans;
        [SerializeField]
        private Image _image;

        private void Start()
        {
            if (!IsOwner)
            {
                Destroy(gameObject); 
                return;
            }

            _player.OnPlayerInitialize.AddListener(Player_OnPlayerInitilize);
        }

        public override void OnDestroy()
        {
            if(IsOwner)
            {
                _player.Weapon.OnUpdateCursorPosition -= PlayerWeapon_OnUpdateCursorPosition;
                _player.Weapon.OnBulletPowerChange -= PlayerWeapon_OnBulletPowerChange;
            }
        }

        private void Player_OnPlayerInitilize(Player player)
        {
            _player.Weapon.OnUpdateCursorPosition += PlayerWeapon_OnUpdateCursorPosition;
            _player.Weapon.OnBulletPowerChange += PlayerWeapon_OnBulletPowerChange;
        }

        private void PlayerWeapon_OnUpdateCursorPosition(object sender, Vector2 e)
        {
            _cursorTrans.position = e;
        }

        private void PlayerWeapon_OnBulletPowerChange(object sender, float e)
        {
            _image.fillAmount = e;
            _image.color = Color.Lerp(Color.white, Color.red, _image.fillAmount);
            float scaleLerp = Mathf.Lerp(1f, 1.5f, _image.fillAmount);
            _cursorTrans.localScale = Vector3.one * scaleLerp;
        }
    }
}
