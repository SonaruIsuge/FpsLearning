using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GunAction : IWeaponAction
{
    public GunData Data;
    
    private Transform weaponHolder;
    private Transform playerCamera;
    private List<Camera> playerCams; 

    private bool _scoping;
    private bool _reloading;
    private bool _shooting;
    private int _ammo;

    private void Awake()
    {
        _ammo = Data.maxAmmo;
    }
    
    private void Update()
    {
        if(!_scoping)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Data.resetSmooth * Time.deltaTime);

            foreach (var cam in playerCams)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Data.defaultFov, Data.fovSmooth * Time.deltaTime);
            }
        }
    }

    public override void OnLeftMouseDown()
    {
        Fire();
    }

    public override void OnLeftMouse()
    {
        if(Data.tapable) return;

        Fire();
    }

    public override void OnLeftMouseUp()
    {

    }
    
    public override void OnRightMouseDown()
    {

    }

    public override void OnRightMouse()
    {
        Scope();
    }

    public override void OnRightMouseUp()
    {
        ExitScope();
    }

    public override void OnKeyRDown()
    {
        if(_scoping) ExitScope();
        Reload();
    }

#region Fire

    private async void Fire()
    {
        if(_ammo <= 0 || _reloading || _shooting) return;
        _shooting = true;
        _ammo--;
        transform.localPosition += new Vector3(0, 0, Data.kickbackForce);

        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out var hitInfo, Data.range, 1<<6 | 1<<7))
        {
            var rb = hitInfo.transform.GetComponent<Rigidbody>();
            if(rb != null) rb.velocity += playerCamera.forward * Data.hitForce;
        }

        await Task.Delay(1000 / Data.shotsPerSecond);
        _shooting = false;
    }

#endregion

#region Reload

    private async void Reload()
    {
        if(_ammo >= Data.maxAmmo || _reloading) return;
        
        _reloading = true;

        await Task.Delay((int)(Data.reloadSpeed * 1000));
        _ammo = Data.maxAmmo;
        _reloading = false;
    }

#endregion

#region  Scope

    private void Scope()
    {
        if(_reloading) return;

        _scoping = true;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.Lerp(transform.localPosition, Data.scopePos, Data.resetSmooth * Time.deltaTime);

        foreach (var cam in playerCams)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Data.scopeFov, Data.fovSmooth * Time.deltaTime);
        }
    }

    private void ExitScope()
    {
        _scoping = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Data.resetSmooth * Time.deltaTime);
    }

#endregion

    public override void SetPlayerParts(Transform weaponHolder, Transform playerCamera, Camera[] playerCams)
    {
        this.weaponHolder = weaponHolder;
        this.playerCamera = playerCamera;
        
        this.playerCams = new List<Camera>();
        foreach(var cam in playerCams) this.playerCams.Add(cam);
    }


    public override int GetAmmo() => _ammo;
}
