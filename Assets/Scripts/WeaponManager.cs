using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour

    
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weapons;

    public GameObject activeWeapon;

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

        public void PickUpWeapon(Interactable gameObject)
        {
        AddWeaponIntoActiveSlot(gameObject);
        }

    private void AddWeaponIntoActiveSlot(Interactable gameObject)
    {

        DropCurrentWeapon(gameObject);
        gameObject.transform.SetParent(activeWeapon.transform, false);

        BulletWeapon weapon = gameObject.GetComponent<BulletWeapon>();
        gameObject.transform.localPosition = new Vector3(weapon.spawnPos.x,weapon.spawnPos.y,weapon.spawnPos.y);
        gameObject.transform.localRotation = Quaternion.Euler(weapon.spawnRot.x, weapon.spawnRot.y, weapon.spawnRot.y);
        gameObject.transform.localScale = new Vector3(weapon.spawnScale.x, weapon.spawnScale.y, weapon.spawnScale.z);
        weapon.isActiveWeapon = true;
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

            
        }
    }

    public void SwitchActiveWeapon(int slotnumber)
    {
        if (activeWeapon.transform.childCount > 0)
        {
            BulletWeapon weapon = activeWeapon.transform.GetChild(0).GetComponent<BulletWeapon>();
            weapon.isActiveWeapon = false;
        }

        activeWeapon = weapons[slotnumber];

        if (activeWeapon.transform.childCount > 0)
        {
            BulletWeapon newWeapon = activeWeapon.transform.GetChild(0).GetComponent<BulletWeapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
}
