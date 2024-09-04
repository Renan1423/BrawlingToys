using UnityEngine;

namespace BrawlingToys.Actors
{
    public class PlayerAttack : MonoBehaviour
    {

        private ICommand _currentShootingCommand;
        private ICommand _meleeCommand;
        PlayerInputs _playerInputs;

        [Header(" - Shooting Cooldown - ")]
        [SerializeField] private float _cooldown = 1.5f;
        [SerializeField] private float _timer = 0.0f;

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
        }

        private void Start()
        {
            _timer = _cooldown;
            _playerInputs.OnShootAction += PlayerInputs_OnAttackAction;
        }

        private void PlayerInputs_OnAttackAction(object sender, System.EventArgs e)
        {
            if (_timer >= _cooldown)
            {
                _currentShootingCommand?.Execute();
                _timer = 0.0f;
            }
            else
            {
                _meleeCommand?.Execute();
            }
        }

        public void SetShootingCommand(ICommand command)
        {
            _currentShootingCommand = command;
        }
        public void SetMeleeCommand(ICommand command)
        {
            _meleeCommand = command;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }
    }
}
