using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions, PlayerInput.IUIActions
{
    private PlayerInput _playerInput;

    public event Action<Vector2> MovePerformedEvent;
    public event Action<Vector2> MoveCancelledEvent;
    public event Action ConfirmEvent;
    public event Action DeclineEvent;

    private void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.SetCallbacks(this);
        _playerInput.UI.SetCallbacks(this);
        SetPlayer();
    }

    public void SetPlayer()
    {
        _playerInput.Player.Enable();
        _playerInput.UI.Disable();
    }

    public void SetUI()
    {
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MovePerformedEvent?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            MoveCancelledEvent?.Invoke(Vector2.zero);
        }
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ConfirmEvent?.Invoke();
        }
    }

    public void OnDecline(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            DeclineEvent?.Invoke();
        }
    }
    
    public void OnNavigate(InputAction.CallbackContext context)
    {
        MovePerformedEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ConfirmEvent?.Invoke();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            DeclineEvent?.Invoke();
        }
    }
}
