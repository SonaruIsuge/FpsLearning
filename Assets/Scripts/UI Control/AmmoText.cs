using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    private TMP_Text ammoText;
    public Player player;
    private PlayerWeaponManager playerWeaponManager;
    private IWeaponAction currentWeapon;

    private void Awake()
    {
        playerWeaponManager = player.GetWeaponManager();
        ammoText = this.GetComponent<TMP_Text>();
        currentWeapon = null;
    }

    private void OnEnable()
    {
        playerWeaponManager.OnCurrentWeaponChange += SetWeapon;
    }

    private void OnDisable()
    {
        playerWeaponManager.OnCurrentWeaponChange -= SetWeapon;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(currentWeapon != null)
        {
            var gunWeapon = currentWeapon as GunAction;
            if(gunWeapon)
                ammoText.text = currentWeapon.GetAmmo() + "/" + gunWeapon.Data.maxAmmo;
        }
        else
        {
            ammoText.text = "";
        }
    }

    private void SetWeapon(IWeaponAction currentWeapon)
    {
        this.currentWeapon = currentWeapon;
    }
}
