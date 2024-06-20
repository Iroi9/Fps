using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;


public class WeaponManager : MonoBehaviour

    
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weapons;

    public GameObject activeWeapon;

    public int slot = 0;

    private void Awake()
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

    private void Start()
    {
        activeWeapon = weapons[0];
    }

    private void Update()
    {
        foreach (GameObject weapon in weapons)
        {
            if (weapon == activeWeapon)
            {
                weapon.SetActive(true);
            }
            else
            {
                weapon.SetActive(false);
            }
        }
    }

    public void PickUpWeapon(GameObject gameObject)
    {
        
        if (weapons.ElementAtOrDefault(1).GetComponentInChildren<BulletWeapon>() == default && weapons[slot].GetComponentInChildren<BulletWeapon>() != null)
        {
            SwitchActiveWeapon(1);
        }
        AddWeaponIntoActiveSlot(gameObject);
        if(weapons.ElementAtOrDefault(0).GetComponentInChildren<BulletWeapon>() != default)
        {
            HUDManager.Instance.GetSprites(gameObject.GetComponent<BulletWeapon>().weaponModel);
        }


    }

    private void AddWeaponIntoActiveSlot(GameObject gameObject)
    {
        BulletWeapon weapon = gameObject.GetComponent<BulletWeapon>();
        DropCurrentWeapon(weapon);
        gameObject.transform.SetParent(activeWeapon.transform, false);
        gameObject.transform.localPosition = new Vector3(weapon.spawnPos.x,weapon.spawnPos.y,weapon.spawnPos.z);
        gameObject.transform.localRotation = Quaternion.Euler(weapon.spawnRot.x, weapon.spawnRot.y, weapon.spawnRot.z);
        gameObject.transform.localScale = new Vector3(weapon.spawnScale.x, weapon.spawnScale.y, weapon.spawnScale.z);
        gameObject.layer = LayerMask.NameToLayer("WeaponRender");
        weapon.isActiveWeapon = true;
        Destroy(weapon.GetComponent<Rigidbody>());
        weapon.animator.enabled = true;
        int childCount = weapon.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            weapon.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("WeaponRender");
        }

    }

    private void DropCurrentWeapon(Interactable interactable)
    {
        if (activeWeapon.transform.childCount > 0)
        {
            var weaponToDrop = activeWeapon.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<BulletWeapon>().isActiveWeapon = false;
            weaponToDrop.transform.SetParent(interactable.transform.parent);
            weaponToDrop.transform.localPosition = interactable.transform.localPosition;
            weaponToDrop.transform.localRotation = interactable.transform.localRotation;
            weaponToDrop.transform.localScale= interactable.transform.localScale;
            weaponToDrop.AddComponent<Rigidbody>();
            weaponToDrop.GetComponent<BulletWeapon>().animator.enabled = false;
            weaponToDrop.layer = LayerMask.NameToLayer("Interactable");
            int childCount = weaponToDrop.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                weaponToDrop.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Interactable");
            }

            weaponToDrop.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void DropCurrentWeapon()
    {
        if (activeWeapon.transform.childCount > 0)
        {
            var weaponToDrop = activeWeapon.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<BulletWeapon>().isActiveWeapon = false;
            while(weaponToDrop.transform.parent != null)
            {
                weaponToDrop.transform.parent = weaponToDrop.transform.parent.parent;
            }
            weaponToDrop.layer = LayerMask.NameToLayer("Interactable");
            int childCount = weaponToDrop.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                weaponToDrop.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Interactable");
            }
            weaponToDrop.GetComponent<BulletWeapon>().animator.enabled = false;
            weaponToDrop.AddComponent<Rigidbody>();
            weaponToDrop.GetComponent<Rigidbody>().mass = 2f;
            weaponToDrop.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 400);
            weaponToDrop.GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 400);


        }
    }
    public void SwitchActiveWeapon(int slotnumber)
    {
        if (activeWeapon != null)
        {
            BulletWeapon weapon = activeWeapon.transform.GetChild(0).GetComponent<BulletWeapon>();
       
            if (activeWeapon.transform.childCount > 0)
            {   
                weapon.isActiveWeapon = false;
                if(slot == 0)
                {
                    slot = 1;
                }
                else
                {
                    slot = 0;
                }
            }

        activeWeapon = weapons[slotnumber];

        if (activeWeapon.transform.childCount > 0)
        {
            BulletWeapon newWeapon = activeWeapon.transform.GetChild(0).GetComponent<BulletWeapon>();
            if (newWeapon != null && weapon != newWeapon)
            {
                HUDManager.Instance.GetSprites(newWeapon.GetComponent<BulletWeapon>().weaponModel);
 
            }
            newWeapon.isActiveWeapon = true;
            }
        }
    }
}
