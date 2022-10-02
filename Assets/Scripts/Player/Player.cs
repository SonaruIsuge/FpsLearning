using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterProperty property;

    public Transform groundCheck;
    public Transform playerHead;
    public Transform weaponHolder;
    public Transform swayHolder;
    public Transform playerCamera;
    public Camera[] playerCams;

    [SerializeField] private IInput playerInput;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerWeaponManager playerWeaponManager;

    private void OnEnable() 
    {
        playerInput.playerInputMaster.Player.Enable();
    }

    private void OnDisable() 
    {
        playerInput.playerInputMaster.Player.Disable();
    }

    private void Awake() 
    {
        playerInput = new PlayerInput();  
        playerLook = new PlayerLook(playerInput, this.transform, playerHead, property);  
        playerMovement = new PlayerMovement(playerInput, this.transform, groundCheck, property);
        playerWeaponManager = new PlayerWeaponManager(playerInput, weaponHolder, swayHolder, playerCamera, playerCams, property);
    }

    void Start()
    {
        
    }

    void Update()
    {
        playerInput.ReadInput();
        playerLook.Tick();
        playerMovement.Tick();
        playerWeaponManager.Tick();

        // if(Input.GetJoystickNames().Length > 0)
        // {
        //     Debug.Log("Using Controller");
        // }
        // else
        // {
        //     Debug.Log("Using keyboard");
        // }
        // foreach(var joystick in Input.GetJoystickNames())
        // {
        //     print("`" + joystick + "`");
        // }
        // Debug.Log(Input.GetJoystickNames().Length);
    }

    public PlayerWeaponManager GetWeaponManager() => playerWeaponManager;

}
