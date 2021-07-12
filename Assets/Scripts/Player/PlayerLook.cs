using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook 
{
    private IInput input;
    private Transform playerBody;
    private CharacterProperty playerProperty;
    private Transform playerHead;
    // private Transform weaponManager;

    private float mouseX;
    private float mouseY;

    private float xRotation = 0f;

    public PlayerLook(IInput input, Transform playerBody, Transform playerHead, CharacterProperty playerProperty)
    {
        this.input = input;
        this.playerBody = playerBody;
        this.playerProperty = playerProperty;
        this.playerHead = playerHead;

        // weaponManager = playerBody.GetComponentInChildren<WeaponManager>().transform;
    }

    public void Tick()
    {
        mouseX = input.MouseX * playerProperty.MouseSensitivity * Time.deltaTime;
        mouseY = input.MouseY * playerProperty.MouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;

        //Joystick
        xRotation -= input.lookRotate.y;

        xRotation = Mathf.Clamp(xRotation, -75f,75f);

        playerBody.Rotate(Vector3.up * mouseX);
        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // weaponManager.localRotation = Quaternion.Euler(-xRotation, 180f, 0f);

        //Joystick 
        playerBody.Rotate(Vector3.up * input.lookRotate.x);
    }
}
