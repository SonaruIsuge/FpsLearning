using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement 
{
    private float GRAVITY = -19.6f;
    private float groundDistance = 0.4f;
    private Vector3 velocity;
    private bool isGrounded;

    private IInput input;
    private Transform player;
    private Transform groundCheck;
    private CharacterProperty playerProperty;
    private CharacterController controller;

    public PlayerMovement(IInput input, Transform player, Transform groundCheck, CharacterProperty playerProperty)
    {
        this.input = input;
        this.player = player;
        this.groundCheck = groundCheck;
        this.playerProperty = playerProperty;
        this.controller = player.GetComponent<CharacterController>();
    }

    public void Tick()
    {
        HandleGravity();
        HandleJump();

        Vector3 move = player.forward * input.Vertical + player.right * input.Horizontal;

        controller.Move(move * playerProperty.MoveSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, LayerMask.GetMask("Ground"));

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // v = g * t * t
        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if(input.Jump > 0 && isGrounded)
        {
            // v = √ ( h * -2 * g )
            velocity.y = Mathf.Sqrt(playerProperty.JumpHeight * -2f * GRAVITY);
        }
    }
}
