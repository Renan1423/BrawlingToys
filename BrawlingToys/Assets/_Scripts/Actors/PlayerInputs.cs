using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerInputs : NetworkBehaviour
{
    // IMPORTANTE - O Update Mode do Input System Package est� setado para : Process Events In Dynamic Update.

    /// <summary>
    ///  - Essa classe será responsável por comunicar, 
    /// através de eventos, quando ações (inputs) do PlayerInputSystem forem performadas.
    /// </summary>

    public event EventHandler OnAttackAction; 
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

    private void InputsInitialization()
    {
        _playerInputActions = new();

        // NOTA : Habilitar o player map por aqui talvez não seja a melhor das opções.
        // Dificultaria controlar os estados de gameplay e menu.
        _playerInputActions.PlayerMap.Enable();

        _playerInputActions.PlayerMap.Move.performed += MovePerformed;
        _playerInputActions.PlayerMap.Attack.performed += AttackPerformed;
        _playerInputActions.PlayerMap.Dash.performed += DashPerformed;
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GetMovementVectorNormalized();
    }

    private void DashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void AttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
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
