using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GrenadeAction : IWeaponAction
{
    public UtilityData Data;

    private Transform weaponHolder;
    private Transform playerCamera;
    private List<Camera> playerCams;

    private Rigidbody rb;

    private bool throwAttack;

    private void Awake()
    {
        throwAttack = false;
    }
    
    private void Update()
    {
        
    }

    public override void OnKeyRDown()
    {
        //Draw Line
    }

    public override void OnLeftMouse()
    {
        
    }

    public override void OnLeftMouseDown()
    {
        
    }

    public override void OnLeftMouseUp()
    {
        // Throw
        throwAttack = true;

        var weaponPickDrop = GetComponent<WeaponPickDrop>();
        if(weaponPickDrop == null) return;

        rb = transform.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.1f;
        // var forward = playerCamera.forward;
        // forward.y = 0f;
        // rb.velocity = forward * Data.forwardForce;
        // rb.velocity += Vector3.up * Data.UpwardForce;

        transform.parent = null;
        rb.angularVelocity += Random.onUnitSphere * Data.rotationForce;
        rb.AddForce(playerCamera.forward * Data.forwardForce, ForceMode.Impulse);

        foreach (var col in weaponPickDrop.gfxColliders)
        {
            col.enabled = true;
        }
        foreach (var gfx in weaponPickDrop.weaponGfxs)
        {
            //Layer = Weapon
            gfx.layer = 7;
        }

        Explode();
    }

    public override void OnRightMouse()
    {
        
    }

    public override void OnRightMouseDown()
    {
        
    }

    public override void OnRightMouseUp()
    {
        
    }

    public override void SetPlayerParts(Transform weaponHolder, Transform playerCamera, Camera[] playerCams)
    {
        this.weaponHolder = weaponHolder;
        this.playerCamera = playerCamera;
        
        this.playerCams = new List<Camera>();
        foreach(var cam in playerCams) this.playerCams.Add(cam);
    }

    public override int GetAmmo() => 1;

    private async void Explode()
    {
        await Task.Delay((int)(Data.delayTime * 1000));

        Collider[] colliders = Physics.OverlapSphere(transform.position, Data.explodeRadius);

        foreach(var near in colliders)
        {
            Rigidbody rb = near.GetComponent<Rigidbody>();
            if(rb) rb.AddExplosionForce(Data.explodeForce, transform.position, Data.explodeRadius, 1f, ForceMode.Impulse);
        }

        Destroy(this.gameObject);
    }
}
