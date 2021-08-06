using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GunData weaponData;
    
    public int weaponGfxLayer;
    public GameObject[] weaponGfxs;
    public Collider[] gfxColliders;

    private float _time;
    private bool _held;
    
    private bool _scoping;
    private bool _reloading;
    private bool _shooting;
    private int _ammo;
    private Rigidbody _rb;
    private Transform _playerCamera;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 0.1f;
        _ammo = weaponData.maxAmmo;
    }

    private void Update() 
    {
        if(!_held) return;

        if(_time < weaponData.animTime)
        {
            _time += Time.deltaTime;
            _time = Mathf.Clamp(_time, 0f, weaponData.animTime);
            var delta = -(Mathf.Cos(Mathf.PI * (_time / weaponData.animTime)) - 1f) / 2f;
            transform.localPosition = Vector3.Lerp(_startPosition, Vector3.zero, delta);
            transform.localRotation = Quaternion.Lerp(_startRotation, Quaternion.identity, delta);
        }
        else 
        {
            _scoping = Input.GetMouseButton(1) && !_reloading;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _scoping ? weaponData.scopePos : Vector3.zero, weaponData.resetSmooth * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if( ( weaponData.tapable? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0) ) && !_reloading && !_shooting)
        {
            if(_ammo <= 0) return;
            _ammo--;
            FireDetail();
            _shooting = true;
            Invoke("ResetShooting", 1f / weaponData.shotsPerSecond);
        }    
    }

    public void Fire()
    {

    }

    public void Reload()
    {
        if(!_shooting && _ammo < weaponData.maxAmmo)
        {
            ReloadDetail();
        }
    }

    public void Scope()
    {
        
    }

    private void FireDetail()
    {
        transform.localPosition += new Vector3(0, 0, weaponData.kickbackForce);

        if(!Physics.Raycast(_playerCamera.position, _playerCamera.forward, out var hitInfo, weaponData.range, 1<<6 | 1<<7)) return;
        
        var rb = hitInfo.transform.GetComponent<Rigidbody>();
        if(rb == null) return;

        rb.velocity += _playerCamera.forward * weaponData.hitForce;
    }

#region ShootingCoolDown

    private void ResetShooting()
    {
        _shooting = false;
    }

#endregion

#region Reload

    private void ReloadDetail()
    {
        _reloading = true;
        Invoke("ReloadFinished", weaponData.reloadSpeed);
    }

    private void ReloadFinished()
    {
        _ammo = weaponData.maxAmmo;
        _reloading = false;
    }

#endregion

    public void PickUp(Transform weaponHolder, Transform playerCamera)
    {
        if(_held) return;

        Destroy(_rb);
        transform.parent = weaponHolder;
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;

        foreach (var col in gfxColliders)
        {
            col.enabled = false;
        }
        foreach (var gfx in weaponGfxs)
        {
            //Layer = WeaponGfx
            gfx.layer = weaponGfxLayer;
        }
        _held = true;
        _playerCamera = playerCamera;

        _scoping = false;
    }

    public void Drop(Transform playerCamera)
    {
        if(!_held) return;

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.mass = 0.1f;
        var forward = playerCamera.forward;
        forward.y = 0f;
        _rb.velocity = forward * weaponData.throwForce;
        _rb.velocity += Vector3.up * weaponData.throwExtraForce;
        _rb.angularVelocity += Random.onUnitSphere * weaponData.rotationForce;

        transform.parent = null;
        foreach (var col in gfxColliders)
        {
            col.enabled = true;
        }
        foreach (var gfx in weaponGfxs)
        {
            //Layer = Weapon
            gfx.layer = 7;
        }
        _held = false;
    }

    public int GetAmmo() => _ammo;
    public bool GetScope() => _scoping;
}
