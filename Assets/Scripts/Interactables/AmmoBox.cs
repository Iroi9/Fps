using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{   
    /// <summary>
    /// This Class describes the Interactable of the AmmoBox
    /// </summary>


    ///
    float displayTime;
    
    /// <summary>
    /// The method restores the original promtMessage
    /// after a certain delay
    /// </summary>
    void Update()
    {
        if (displayTime <= 0)
        {
            promtMessage = "Refill Weapon";
        }
        displayTime--;
    }
    /// <summary>
    /// This method handels the interaction with the AmmoBox
    /// If no active Weapon is found recplaces the prompt message accordingly for 300 frames (5 seconds)
    /// other wise refills Ammo
    /// </summary>
    /// <param name="gameObject"></param> is not used, is here because of inheretance
    protected override void Interact(GameObject gameObject)
    {
        BulletWeapon currWeapon = WeaponManager.Instance.activeWeapon.GetComponentInChildren<BulletWeapon>();
        if (currWeapon != null)
        {
            currWeapon.maxAmmo = currWeapon.maxAmmo + currWeapon.magazineSize * 3;
            Destroy(this.gameObject);
        }
        else
        {
            
            promtMessage = "No weapon Equiped";
            displayTime = 300f;
        }
    }
}
