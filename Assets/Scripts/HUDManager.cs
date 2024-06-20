using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    //UI
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeponUI;
    public Image unActiveWeponUI;

    [Header("Throwable")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmounUI;

    public Sprite emptySlot;

    public GameObject crosshair;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void Update()
    {
       
       BulletWeapon activeWeapon = WeaponManager.Instance.activeWeapon.GetComponentInChildren<BulletWeapon>();
       BulletWeapon unActiveWeapon = GetNotActiveWeaponSlot().GetComponent<BulletWeapon>();


        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.maxAmmo / activeWeapon.bulletsPerBurst}";
            BulletWeapon.WeaponModel model = activeWeapon.weaponModel;

        }
        else
        {
           magazineAmmoUI.text = "";
           totalAmmoUI.text = "" ;

           ammoTypeUI.sprite = emptySlot;
           activeWeponUI.sprite = emptySlot;
        }
    }

    public void GetSprites(BulletWeapon.WeaponModel model)
    {
        GameObject ammo = GetAmmoSprite(model);
        ammo.transform.position = new Vector3(1000f,1000f,1000f);
        Destroy(ammo, 2f);
        ammoTypeUI.sprite = ammo.GetComponent<SpriteRenderer>().sprite; ;
        
        GameObject weapon = GetWeaponSprite(model);
        weapon.transform.position = new Vector3(1000f, 1000f, 1000f);
        Destroy(weapon, 2f);
        activeWeponUI.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
    }

    private GameObject GetWeaponSprite(BulletWeapon.WeaponModel model)
    {

        switch (model)
        {
            case BulletWeapon.WeaponModel.Pistol1911:
                return Instantiate(Resources.Load<GameObject>("Pistol1911_Weapon"));
            case BulletWeapon.WeaponModel.AK47:
                return Instantiate(Resources.Load<GameObject>("AK47_Weapon"));
            case BulletWeapon.WeaponModel.BennelliM4:
                return Instantiate(Resources.Load<GameObject>("BennelliM4_Weapon"));

            default:
                return null;
        }
    }

    private GameObject GetAmmoSprite(BulletWeapon.WeaponModel model)
    {
        switch (model)
        {
            case BulletWeapon.WeaponModel.Pistol1911:
                return Instantiate(Resources.Load<GameObject>("Pistol1911_Ammo"));

            case BulletWeapon.WeaponModel.AK47:
                return Instantiate(Resources.Load<GameObject>("AK47_Ammo"));
            case BulletWeapon.WeaponModel.BennelliM4:
                return Instantiate(Resources.Load<GameObject>("BennelliM4_Ammo"));

            default:
                return null;
        }
    }

    private GameObject GetNotActiveWeaponSlot()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weapons)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeapon)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}
