using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set;}
    public Vector2 LookValue { get; private set; }
    public bool SprintValue { get; private set; }
    public bool WalkValue { get; private set; }
    public bool IsShooting { get; private set; }


    public event Action JumpEvent;

    public event Action AimEvent;


    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        
        AimEvent?.Invoke();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        WalkValue = context.ReadValue<bool>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintValue = context.ReadValue<bool>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsShooting = true;
        }
        else if (context.canceled)
        {
            IsShooting = false;
        }
    }
}
