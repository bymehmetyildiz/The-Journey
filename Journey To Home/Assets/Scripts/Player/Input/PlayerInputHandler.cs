using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 rawMovementInput { get; private set; }
    public int normInputX { get; private set; }
    public int normInputY { get; private set; }
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }
    public bool grabInput { get; private set; }
    public bool blockInput { get; private set; }
    public bool dodgeInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool[] attackInputs { get; private set; }

    

    [SerializeField] private float inputHoldTime = 0.2f;
    private float jumpInputStartTime, attackInputTime, blockStartTime, dodgeStartTime, interactInputTime;
   

    private void Start()
    {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        attackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckAttackInputHoldTime();
        CheckBlockHoldTime();
        CheckDodgeHoldTime();
        
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            attackInputs[(int)(CombatInputs.primary)] = true;
            attackInputTime = Time.time;
        }
        if (context.canceled)
        {            
            attackInputs[(int)(CombatInputs.primary)] = false;
        }        

    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {            
            attackInputs[(int)(CombatInputs.secondary)] = true;            
        }
        if (context.canceled)        
        {           
            attackInputs[(int)(CombatInputs.secondary)] = false;
        }
    }

    public void OnDodgeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dodgeStartTime = Time.time;
            dodgeInput = true;
        }
        if (context.canceled)
        {
            dodgeInput = false;
        }

    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactInput = true;
            interactInputTime = Time.time;
        }
        if (context.canceled)
        {
            interactInput = false;
        }

    }


    public void OnBlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            blockInput = true;
            blockStartTime = Time.time;
        }
        if (context.canceled)
        {
            blockInput = false; 
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawMovementInput = context.ReadValue<Vector2>();

        normInputX = (int)(rawMovementInput * Vector2.right).normalized.x;
        normInputY = (int)(rawMovementInput * Vector2.up).normalized.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if(context.canceled)
        {
            jumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabInput = true;
        }
        if (context.canceled)
        {
            grabInput = false;
        }
    }


    public void UseJumpInput() => jumpInput = false;

    public void UseInteractInput() => interactInput = false;

    public void UseAttackInput() => attackInputs[(int)(CombatInputs.primary)] = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time > jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
        }
    }

    private void CheckAttackInputHoldTime()
    {
        if(Time.time > attackInputTime + inputHoldTime)
        {
            attackInputs[(int)(CombatInputs.primary)] = false;
        }
    }  

    private void CheckBlockHoldTime()
    {
        if (Time.time > blockStartTime + inputHoldTime)
        {
            blockInput = false;
        }

    }

    private void CheckDodgeHoldTime()
    {
        if (Time.time > dodgeStartTime + inputHoldTime)
        {
            dodgeInput = false;
        }

    }

  



}

public enum CombatInputs
{
    primary,
    secondary,
}