using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun,
    Utility,
    Melee,
}

[CreateAssetMenu(fileName ="New Weapon Data", menuName ="Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType type;

    [Space()]
    [Header("Pick and Throw")]

    public float throwForce;
    public float throwExtraForce;
    public float rotationForce;
    public float animTime;

    [Space()]
    [Header("Fire")]

    public int maxAmmo;
    public int shotsPerSecond;
    public float reloadSpeed;
    public float hitForce;
    public int damage;
    public float range;
    public bool tapable;
    public float kickbackForce;
    public float resetSmooth;

    [Space()]
    [Header("Scope")]

    public Vector3 scopePos;
    public float defaultFov;
    public float scopeFov;
    public float fovSmooth;    
    public Sprite crossHair;
    public Sprite scopeCrosshair;


}
