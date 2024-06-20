using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    float displayTime;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (displayTime <= 0)
        {
            promtMessage = "Refill Weapon";
        }
        displayTime--;
    }

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
