using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAction : IWeaponAction
{
    public WeaponData data;
    
    private Transform weaponHolder;
    private Transform playerCamera;
    private List<Camera> playerCams; 

    private bool _scoping;
    private bool _reloading;
    private bool _shooting;
    private int _ammo;

    private void Awake()
    {
        _ammo = data.maxAmmo;
    }
    
    private void Update()
    {
        if(!_scoping)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, data.resetSmooth * Time.deltaTime);

            foreach (var cam in playerCams)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, data.defaultFov, data.fovSmooth * Time.deltaTime);
            }
        }
    }

    public override void OnLeftMouseDown()
    {
        Fire();
    }

    public override void OnLeftMouse()
    {
        if(data.tapable) return;

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

    private void Fire()
    {
        if(_ammo <= 0 || _reloading || _shooting) return;
        _shooting = true;
        _ammo--;
        transform.localPosition += new Vector3(0, 0, data.kickbackForce);

        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out var hitInfo, data.range, 1<<6 | 1<<7))
        {
            var rb = hitInfo.transform.GetComponent<Rigidbody>();
            if(rb != null) rb.velocity += playerCamera.forward * data.hitForce;
        }
        
        Invoke("ResetShooting", 1f / data.shotsPerSecond);
    }

    private void ResetShooting()
    {
        _shooting = false;
    }

#endregion

#region Reload

    private void Reload()
    {
        if(_shooting || _ammo >= data.maxAmmo) return;
        
        _reloading = true;
        Invoke("ReloadFinished", data.reloadSpeed);
        
    }

    private void ReloadFinished()
    {
        _ammo = data.maxAmmo;
        _reloading = false;
    }

#endregion

#region  Scope

    private void Scope()
    {
        if(_reloading) return;

        _scoping = true;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.Lerp(transform.localPosition, data.scopePos, data.resetSmooth * Time.deltaTime);

        foreach (var cam in playerCams)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, data.scopeFov, data.fovSmooth * Time.deltaTime);
        }
    }

    private void ExitScope()
    {
        _scoping = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, data.resetSmooth * Time.deltaTime);
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
    public override WeaponData GetData() => data;
}
