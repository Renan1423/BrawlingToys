using UnityEngine;
using BrawlingToys.Actors;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections;

namespace BrawlingToys.UI
{
    public class GunLoadBarUI : NetworkBehaviour
    {
        [SerializeField] private Player _player;

        [SerializeField]
        private RectTransform _cursorTrans;
        [SerializeField]
        private Image _image;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Destroy(gameObject); 
                return;
            }

            StartCoroutine(Action()); 

            IEnumerator Action()
            {
                yield return new WaitUntil(() => _player.Initialized); 

                _player.Weapon.OnUpdateCursorPosition += PlayerWeapon_OnUpdateCursorPosition;
                _player.Weapon.OnBulletPowerChange += PlayerWeapon_OnBulletPowerChange;
            }
        }

        public override void OnDestroy()
        {
            if(IsOwner)
            {
                _player.Weapon.OnUpdateCursorPosition -= PlayerWeapon_OnUpdateCursorPosition;
                _player.Weapon.OnBulletPowerChange -= PlayerWeapon_OnBulletPowerChange;
            }
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
