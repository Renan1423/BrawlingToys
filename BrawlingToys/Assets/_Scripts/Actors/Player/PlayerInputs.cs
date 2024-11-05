using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrawlingToys.Actors
{
    public class PlayerInputs : NetworkBehaviour
    {
        // IMPORTANTE - O Update Mode do Input System Package est� setado para : Process Events In Dynamic Update.

        /// <summary>
        ///  - Essa classe será responsável por comunicar, 
        /// através de eventos, quando ações (inputs) do PlayerInputSystem forem performadas.
        /// </summary>

        public bool IsActive { get; private set; }
        public PlayerInputActions PlayerInputActions { get => _playerInputActions; }

        public event EventHandler OnShootAction;
        public event EventHandler OnMeleeAction;
        public event EventHandler OnDashAction;
        public event EventHandler<Vector2> OnMoveAction;

        private PlayerInputActions _playerInputActions;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                InputsInitialization();
            }
        }

        private void OnEnable()
        {
            if(IsOwner) TogglePlayerMap(true); 
        }

        private void InputsInitialization()
        {
            _playerInputActions = new();

            // NOTA : Habilitar o player map por aqui talvez não seja a melhor das opções.
            // Dificultaria controlar os estados de gameplay e menu.
            TogglePlayerMap(true);

            _playerInputActions.PlayerMap.Move.performed += MovePerformed;
            _playerInputActions.PlayerMap.Shoot.canceled += ShootPerformed;
            _playerInputActions.PlayerMap.Melee.performed += MeleePerformed;
            _playerInputActions.PlayerMap.Dash.performed += DashPerformed;
        }

        public void TogglePlayerMap(bool val)
        {
            IsActive = val;

            if(val)
                EnablePlayerMap();
            else
                _playerInputActions.PlayerMap.Disable();
        }

        public void EnablePlayerMap() {
            _playerInputActions.PlayerMap.Enable();
        }

        private void MovePerformed(InputAction.CallbackContext obj)
        {
            GetMovementVectorNormalized();
        }

        private void DashPerformed(InputAction.CallbackContext obj)
        {
            OnDashAction?.Invoke(this, EventArgs.Empty);
        }

        private void ShootPerformed(InputAction.CallbackContext obj)
        {
            OnShootAction?.Invoke(this, EventArgs.Empty);
        }

        private void MeleePerformed(InputAction.CallbackContext context)
        {
            OnMeleeAction?.Invoke(this, EventArgs.Empty);
        }

        // Metodo respons�vel por ler as entradas de input de movimento e retornar como Vector2 normalizado
        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.PlayerMap.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            OnMoveAction?.Invoke(this, inputVector);
            return inputVector;
        }

        public Vector2 GetLookVector()
        {
            Vector2 inputVector = _playerInputActions.PlayerMap.Look.ReadValue<Vector2>();

            return inputVector;
        }
    }
}
