using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager
{
    //武器Layer
    private int WeaponLayer = 7;

    private IInput input;
    private CharacterProperty playerProperty;
    private Transform weaponHolder;
    private Transform swayHolder;
    private Transform playerCamera;
    private Camera[] playerCams;

    private bool _isWeaponHeld;
    private WeaponPickDrop _heldWeapon;
    private IWeaponAction _weaponAction;

    public event Action<IWeaponAction> OnCurrentWeaponChange;

    public PlayerWeaponManager(IInput input, Transform weaponHolder, Transform swayHolder, Transform playerCamera, Camera[] playerCams, CharacterProperty property)
    {
        this.input = input;
        this.weaponHolder = weaponHolder;
        this.swayHolder = swayHolder;
        this.playerCamera = playerCamera;
        this.playerProperty = property;
        this.playerCams = playerCams;
    }

    public void Tick()
    {
        
        //sway
        if(_isWeaponHeld)
        {
            var mouseDelta = new Vector2(input.MouseX, input.MouseY);
            swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, playerProperty.swaySmooth * Time.deltaTime);
            swayHolder.localPosition += (Vector3)mouseDelta * playerProperty.swaySize;
        }

        //Pickup
        if(input.Interact)
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, playerProperty.pickupRange, 1<<6 | 1<<7);
            if(hit.transform == null || hit.transform.gameObject.layer != WeaponLayer) return;
            Debug.Log(hit.transform?.gameObject.name);
            _isWeaponHeld = true;
            if(_heldWeapon) _heldWeapon.DropWeapon(playerCamera);
            _heldWeapon = hit.transform.gameObject.GetComponent<WeaponPickDrop>();
            _weaponAction = hit.transform.gameObject.GetComponent<IWeaponAction>();
            _heldWeapon.PickUpWeapon(weaponHolder, playerCamera, playerCams);
            Debug.Log(_weaponAction);
            if(OnCurrentWeaponChange!= null) OnCurrentWeaponChange.Invoke(_weaponAction);
        }

        //Drop
        if(input.Drop)
        {
            if(!_heldWeapon) return;

            _heldWeapon.DropWeapon(playerCamera);
            _heldWeapon = null;
            _isWeaponHeld = false;

            if(OnCurrentWeaponChange!= null) OnCurrentWeaponChange.Invoke(null);
        }

        //Fire
        if(input.TapShoot) 
        {
            if(_weaponAction == null) return;

            _weaponAction.OnLeftMouseDown();
        }

        if(input.HoldShoot)
        {
            if(_weaponAction == null) return;

            _weaponAction.OnLeftMouse();
        }

        if(input.ExitShoot)
        {   
            if(_weaponAction == null) return;

            _weaponAction.OnLeftMouseUp();
        }

        //Reload
        if(input.Relaod)
        {
            if(_weaponAction == null) return;

            _weaponAction.OnKeyRDown();
        }

        //Scope
        if(input.TapScope)
        {
            if(_weaponAction == null) return;

            _weaponAction.OnRightMouseDown();
        }

        if(input.Scope)
        {
            if(_weaponAction == null) return;

            _weaponAction.OnRightMouse();
            
        }

        if(input.ExitScope)
        {
            if(_weaponAction == null) return;

            _weaponAction.OnRightMouseUp();
        }
        
    }

    public WeaponPickDrop GetWeapon => _heldWeapon;
}
