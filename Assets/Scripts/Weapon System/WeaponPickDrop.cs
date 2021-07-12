using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickDrop : MonoBehaviour
{
    public WeaponData data;
    public List<GameObject> weaponGfxs;
    public List<Collider> gfxColliders;
    public int weaponGfxLayer = 8;

    // private IWeaponAction weaponAction;
    private Rigidbody rb;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _held;
    private float _time;

    private IWeaponAction weaponAction;

    private void Awake()
    {
        // weaponAction = GetComponent<IWeaponAction>();

        rb = GetComponent<Rigidbody>();
        rb.mass = 0.1f;

        _held = false;

        weaponAction = GetComponent<IWeaponAction>();
        if(weaponAction) weaponAction.enabled = false;
    }

    private void Update()
    {
        if(_held && _time < data.animTime)
        {
            _time += Time.deltaTime;
            _time = Mathf.Clamp(_time, 0f, data.animTime);
            var delta = -(Mathf.Cos(Mathf.PI * (_time / data.animTime)) - 1f) / 2f;
            transform.localPosition = Vector3.Lerp(_startPosition, Vector3.zero, delta);
            transform.localRotation = Quaternion.Lerp(_startRotation, Quaternion.identity, delta);
        }
        else if(_held)
        {
            if(weaponAction) weaponAction.enabled = true;
        }
    }

    public void PickUpWeapon(Transform weaponHolder, Transform playerCamera, Camera[] playerCams)
    {
        if(_held) return;

        GameObject.Destroy(rb);
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

        if(weaponAction) weaponAction.SetPlayerParts(weaponHolder, playerCamera, playerCams);
    }

    public void DropWeapon(Transform playerCamera)
    {
        if(!_held) return;

        rb = transform.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.1f;
        var forward = playerCamera.forward;
        forward.y = 0f;
        rb.velocity = forward * data.throwForce;
        rb.velocity += Vector3.up * data.throwExtraForce;
        rb.angularVelocity += Random.onUnitSphere * data.rotationForce;

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

        if(weaponAction) weaponAction.enabled = false;
    }
}
