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

    [Header("Placeholder")]
    public Sprite emptySlot;
    public Sprite greySlot;
    
    [Header("Crosshair")]
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
    /// <summary>
    /// Each Frame this method updates the ammo count
    /// and ceck if it is neccesry to load the apropriet sprites
    /// </summary>
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

        if (WeaponManager.Instance.letahls <= 0)
        {
            lethalUI.sprite = greySlot;
        }

        if (WeaponManager.Instance.tacticals <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }
    /// <summary>
    /// This method loads the WeaponSpite
    /// </summary>
    /// <param name="model"></param> is needed to load the correct sprite
    public void GetSprites(BulletWeapon.WeaponModel model)
    {
        GameObject ammo = GetAmmoSprite(model);
       
        ammoTypeUI.sprite = ammo.GetComponent<SpriteRenderer>().sprite; ;
        
        GameObject weapon = GetWeaponSprite(model);
        activeWeponUI.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// Loads the Weapon Sprites
    /// </summary>
    /// <param name="model"></param> is nedded the get the correct sprite
    /// <returns></returns>
    private GameObject GetWeaponSprite(BulletWeapon.WeaponModel model)
    {

        switch (model)
        {
            case BulletWeapon.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon");
            case BulletWeapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("AK47_Weapon");
            case BulletWeapon.WeaponModel.BennelliM4:
                return Resources.Load<GameObject>("BennelliM4_Weapon");
            case BulletWeapon.WeaponModel.RPG7:
                return Resources.Load<GameObject>("RPG7_Weapon");

            default:
                return null;
        }
    }
    /// <summary>
    /// Loads the Ammo Sprites
    /// </summary>
    /// <param name="model"></param> is nedded the get the correct sprite
    /// <returns></returns>
    private GameObject GetAmmoSprite(BulletWeapon.WeaponModel model)
    {
        switch (model)
        {
            case BulletWeapon.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Ammo");

            case BulletWeapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("AK47_Ammo");
            case BulletWeapon.WeaponModel.BennelliM4:
                return Resources.Load<GameObject>("BennelliM4_Ammo");
            case BulletWeapon.WeaponModel.RPG7:
                return Resources.Load<GameObject>("RPG7_Ammo");

            default:
                return null;
        }
    }
    /// <summary>
    /// Get the not Active Weapon slot
    /// </summary>
    /// <returns>The weapon slot that is not active</returns>
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

   public void UpdateThorwableUI()
   {
        lethalAmountUI.text = $"{WeaponManager.Instance.letahls}";
        tacticalAmounUI.text = $"{WeaponManager.Instance.tacticals}";
        switch (WeaponManager.Instance.equipedLethal)
        {
            case Throwable.ThowableTyp.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.Instance.equipedTactical)
        {
            case Throwable.ThowableTyp.Smoke:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
