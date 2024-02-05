using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerActions _input;
    
    public Vector2 MoveLeftValue { get; private set; } = Vector2.zero;
    public Vector2 MoveRightValue { get; private set; } = Vector2.zero;
    public bool SuctionLeftHeld { get; private set; }
    public bool SuctionRightHeld { get; private set; }
    public bool SuctionLeftPressed { get; private set; }
    public bool SuctionRightPressed { get; private set; }

    private void OnEnable()
    {
        _input = new PlayerActions();
        _input.Enable();
        _input.gameplay.MoveLeft.performed += MoveLeft;
        _input.gameplay.MoveLeft.canceled += MoveLeft;
        
        _input.gameplay.MoveRight.performed += MoveRight;
        _input.gameplay.MoveRight.canceled += MoveRight;

        _input.gameplay.SuctionLeft.started += SuctionLeft;
        _input.gameplay.SuctionLeft.performed += SuctionLeft;
        _input.gameplay.SuctionLeft.canceled += SuctionLeft;
        
        _input.gameplay.SuctionRight.started += SuctionRight;
        _input.gameplay.SuctionRight.performed += SuctionRight;
        _input.gameplay.SuctionRight.canceled += SuctionRight;
    }
    
    private void OnDisable()
    {
        
        _input.gameplay.MoveLeft.performed -= MoveLeft;
        _input.gameplay.MoveLeft.canceled -= MoveLeft;
        
        _input.gameplay.MoveRight.performed -= MoveRight;
        _input.gameplay.MoveRight.canceled -= MoveRight;


        _input.gameplay.SuctionLeft.started -= SuctionLeft;
        _input.gameplay.SuctionLeft.performed -= SuctionLeft;
        _input.gameplay.SuctionLeft.canceled -= SuctionLeft;
        
        _input.gameplay.SuctionRight.started -= SuctionRight;
        _input.gameplay.SuctionRight.performed -= SuctionRight;
        _input.gameplay.SuctionRight.canceled -= SuctionRight;
        
        _input.gameplay.Disable();
    }

    private void MoveLeft(InputAction.CallbackContext ctx)
    {
        MoveLeftValue = ctx.ReadValue<Vector2>();
    }
    
    private void MoveRight(InputAction.CallbackContext ctx)
    {
        MoveRightValue = ctx.ReadValue<Vector2>();
    }

    private void SuctionRight(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("right pressed");
            SuctionRightPressed = true;
            SuctionRightHeld = true;
        }
        else if (ctx.performed)
        {
            Debug.Log("right held");
            SuctionRightPressed = false;
            SuctionRightHeld = true;
        }
        else if (ctx.canceled)
        {
            Debug.Log("right released");
            SuctionRightPressed = false;
            SuctionRightHeld = false;
        }
    }

    private void SuctionLeft(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("Left pressed");
            SuctionLeftPressed = true;
            SuctionLeftHeld = true;
        }
        else if (ctx.performed)
        {
            Debug.Log("Left held");
            SuctionLeftPressed = false;
            SuctionLeftHeld = true;
        }
        else if (ctx.canceled)
        {
            Debug.Log("Left released");
            SuctionLeftPressed = false;
            SuctionLeftHeld = false;
        }
    }
    
}
    
