using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeaponAction : MonoBehaviour
{
    public abstract void OnLeftMouseDown();
    public abstract void OnLeftMouse();
    public abstract void OnLeftMouseUp();
    public abstract void OnRightMouseDown();
    public abstract void OnRightMouse();
    public abstract void OnRightMouseUp();
    public abstract void OnKeyRDown();

    public abstract void SetPlayerParts(Transform weaponHolder, Transform playerCamera, Camera[] playerCams);

    public abstract int GetAmmo();
    public abstract WeaponData GetData();
}
