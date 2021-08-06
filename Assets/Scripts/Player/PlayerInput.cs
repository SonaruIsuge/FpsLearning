using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : IInput
{
    public float Horizontal {get; private set;}
    public float Vertical {get; private set;}
    public float MouseX {get; private set;}
    public float MouseY {get; private set;}
    public float Jump {get; private set;}
    public Vector2 lookRotate {get; private set;}

    public bool Interact {get; private set;}
    public bool TapShoot {get; private set;}
    public bool HoldShoot {get; private set;}
    public bool ExitShoot {get; private set;}
    public bool TapScope {get; private set;}
    public bool Scope {get; private set;}
    public bool ExitScope {get; private set;}
    public bool Relaod {get; private set;}
    public bool Drop {get; private set;}
    public bool Alpha1 {get; private set;}
    public bool Alpha2 {get; private set;}
    public bool Alpha3 {get; private set;}



    public InputMaster playerInputMaster {get; private set;}

    public PlayerInput()
    {
        playerInputMaster = new InputMaster();

        playerInputMaster.Player.Rotate.performed += ctx => lookRotate = ctx.ReadValue<Vector2>();
        playerInputMaster.Player.Rotate.canceled += ctx => lookRotate = Vector2.zero;
        playerInputMaster.Player.Jump.performed += ctx => Jump = ctx.ReadValue<float>();
        playerInputMaster.Player.Jump.canceled += ctx => Jump = 0f;
    }

    public void ReadInput()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        Jump = Input.GetAxis("Jump");
        
        TapShoot = Input.GetMouseButtonDown(0);
        HoldShoot = Input.GetMouseButton(0);
        ExitShoot = Input.GetMouseButtonUp(0);
        TapScope = Input.GetMouseButtonDown(1);
        Scope = Input.GetMouseButton(1);
        ExitScope = Input.GetMouseButtonUp(1);
        
        Interact = Input.GetKeyDown(KeyCode.E);
        Relaod = Input.GetKeyDown(KeyCode.R);
        Drop = Input.GetKeyDown(KeyCode.Q);

        Alpha1 = Input.GetKeyDown(KeyCode.Alpha1);
        Alpha2 = Input.GetKeyDown(KeyCode.Alpha2);
        Alpha3 = Input.GetKeyDown(KeyCode.Alpha3);
    }
}
