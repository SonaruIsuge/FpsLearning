using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public float pickupRange;
    public float pickupRadius;
    public int weaponLayer;

    public float swaySize;
    public float swaySmooth;

    public float defaultFov;
    public float scopeFov;
    public float fovSmooth;

    public Transform weaponHolder;
    public Transform swayHolder;
    public Transform playerCamera;
    
    public Image crosshairImage;

    public Camera[] playerCams;

    private bool _isWeaponHeld;
    private Weapon _heldWeapon;

    // public event Action<Weapon> OnCurrentWeaponChange;

    private void Update() {
        crosshairImage.gameObject.SetActive(!_isWeaponHeld || !_heldWeapon.GetScope());
        foreach (var cam in playerCams)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _isWeaponHeld && _heldWeapon.GetScope() ? scopeFov : defaultFov, fovSmooth * Time.deltaTime);
        }

        if(_isWeaponHeld)
        {
            var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            swayHolder.localPosition = Vector3.Lerp(swayHolder.localPosition, Vector3.zero, swaySmooth * Time.deltaTime);
            swayHolder.localPosition += (Vector3)mouseDelta * swaySize;

            if(Input.GetKeyDown(KeyCode.Q))
            {
                _heldWeapon.Drop(playerCamera);
                _heldWeapon = null;
                _isWeaponHeld = false;
                // if(OnCurrentWeaponChange!= null) OnCurrentWeaponChange.Invoke(null);
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange, 1<<6 | 1<<7);
            if(hit.transform == null || hit.transform.gameObject.layer != weaponLayer) return;
            Debug.Log(hit.transform?.gameObject.name);
            _isWeaponHeld = true;
            _heldWeapon = hit.transform.gameObject.GetComponent<Weapon>();
            if(!_isWeaponHeld) _heldWeapon.PickUp(weaponHolder, playerCamera);

            // if(OnCurrentWeaponChange!= null) OnCurrentWeaponChange.Invoke(_heldWeapon);
        }
    }

    public Weapon GetWeapon() => _heldWeapon;
}
