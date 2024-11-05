using UnityEngine;
using BrawlingToys.Actors;
using UnityEngine.UI;

namespace BrawlingToys.UI
{
    public class GunLoadBarUI : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();

            _player.Weapon.OnUpdateCursorPosition += PlayerWeapon_OnUpdateCursorPosition;
            _player.Weapon.OnBulletPowerChange += PlayerWeapon_OnBulletPowerChange;
        }

        private void PlayerWeapon_OnUpdateCursorPosition(object sender, Vector2 e)
        {
            _image.rectTransform.position = e;
        }

        private void PlayerWeapon_OnBulletPowerChange(object sender, float e)
        {
            _image.fillAmount = e;
        }
    }
}
