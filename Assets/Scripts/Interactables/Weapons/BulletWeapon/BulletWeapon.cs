using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : Interactable
{

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float fireRate;
    public float delay;
    

    //Burst
    public int bulletsPerBurst;
    public int burstBulletsLeft;

    //Spread
    private float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;


    private InputManager playerInput;
    //Bullet
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float bulletVelocity;
    [SerializeField] float bulletLife;
    [SerializeField] bool isReloading;

    //Reload
    [SerializeField] float reloadTime;
    [SerializeField] internal int magazineSize, bulletsLeft;
    internal int maxAmmo;
    //Animations
    public GameObject muzzleEffect;
    internal Animator animator;

    //Pos on Player
    public Vector3 spawnPos;
    public Vector3 spawnRot;
    public Vector3 spawnScale;

    bool isADS;

    public bool isActiveWeapon;
    public enum WeaponModel
    {
        Pistol1911,
        AK47,
        BennelliM4,
        RPG7
        
    }

    public WeaponModel weaponModel;

    public enum WeaponType
    {
        Pistol,
        AssaultRifle,
        Shotgun,
        RocketLauncher
    }
    
    public WeaponType weaponType;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        maxAmmo = magazineSize * 3;
        spreadIntensity = hipSpreadIntensity;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (playerInput == null)
        {
            playerInput = GetComponentInParent<InputManager>();
        }
        else if (playerInput.onFootActions.SwitchWepSlot1.IsPressed() && WeaponManager.Instance.slot != 0 && WeaponManager.Instance.weapons[0].GetComponentInChildren<BulletWeapon>() != null)
        {
            ChangeWeapon(0);
        }
        else if(playerInput.onFootActions.SwitchWepSlot2.IsPressed() && WeaponManager.Instance.slot != 1 && WeaponManager.Instance.weapons[1].GetComponentInChildren<BulletWeapon>() != null)
        {
            ChangeWeapon(1);
        }
        else if (playerInput.onFootActions.Drop.IsPressed())
        {
            DropAction();
        }
        else
        {
            if (isActiveWeapon)
            {

                if (playerInput.onFootActions.Ads.ReadValue<float>() == 1.0 && isADS == false && isReloading == false)
                {
                   EnterAds();
                }
                if(playerInput.onFootActions.Ads.ReadValue<float>() == 0.0 && isADS == true)
                {
                   ExitAds();
                }

                if (bulletsLeft == 0 && isShooting)
                {
                    SoundManager.Instance.emptySound1911.Play();
                }

                if (currShootingMode == ShootingMode.Auto)
                {
                    isShooting = playerInput.onFootActions.Shoot.IsPressed();
                }
                else if (currShootingMode == ShootingMode.Single || currShootingMode == ShootingMode.Burst)
                {
                    isShooting = playerInput.onFootActions.Shoot.triggered;
                }

                if (readyToShoot && isShooting && bulletsLeft > 0 && isReloading == false)
                {
                    burstBulletsLeft = bulletsPerBurst;
                    FireWeapon();

                }

                if (maxAmmo > 0)
                {
                    if (playerInput.onFootActions.Reload.IsPressed() && bulletsLeft < magazineSize && isReloading == false)
                    {
                        Reload();
                    }

                    if (readyToShoot == false && bulletsLeft <= 0)
                    {
                        Reload();
                    }
                }

            }
        }
       
       
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        
        if (weaponType != WeaponType.Shotgun)
        {
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            if (isADS)
            {
                animator.SetTrigger("RECOIL_ADS");
            }
            else
            {
                animator.SetTrigger("RECOIL");
            }
        }
        else
        {
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            if (burstBulletsLeft > bulletsPerBurst-1)
            {
                if (isADS)
                {
                    animator.SetTrigger("RECOIL_ADS");
                }
                else
                {
                    animator.SetTrigger("RECOIL");
                }
            }
        }
        
        
        
        
        
        SoundManager.Instance.PlayShootingSound(weaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpred().normalized; 

        //Init bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Position bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Fire bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        //Destroy bullet after bullet life time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLife));

        //check if we are done Shooting
        if (allowReset) {
            if(fireRate == 0)
            {
                Invoke("ResetShot", delay);
                allowReset = false;
            }
            else 
            {
                Invoke("ResetShot", fireRate);
                allowReset = false;
            }
            
        }
        
        //BurstMode
        if (currShootingMode == ShootingMode.Burst && burstBulletsLeft > 0) {
            burstBulletsLeft--;
            Invoke("FireWeapon", fireRate);
        }
    }

    private void EnterAds()
    {
        animator.SetTrigger("ENTER_ADS");
        isADS = true;
        HUDManager.Instance.crosshair.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }

    private void ExitAds()
    {
        animator.SetTrigger("EXIT_ADS");
        isADS = false;
        HUDManager.Instance.crosshair.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }
    private void Reload()
    {
      
        animator.SetTrigger("RELOAD");
 
        SoundManager.Instance.PlayReloadingSound(weaponModel);
        isReloading = true;
        readyToShoot = false;
        Invoke("ReloadComplet", reloadTime);
    }

    private void ReloadComplet()
    {
        if (maxAmmo - (magazineSize - bulletsLeft) < 0)
        {
            maxAmmo = 0;
        }
        else
        {
            maxAmmo = maxAmmo - (magazineSize - bulletsLeft);
        }
        bulletsLeft = magazineSize;
        isReloading = false;
        readyToShoot = true;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpred()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targepoint;

        if (Physics.Raycast(ray,out hit))
        {
            targepoint = hit.point;
        }else
        {
            targepoint = ray.GetPoint(100);
        }

        Vector3 direction = targepoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x,y,0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletLife)
    {
        yield return new WaitForSeconds(bulletLife);
        Destroy(bullet);
    }

    protected override void Interact(GameObject gameObject)
    {
            WeaponManager.Instance.PickUpWeapon(gameObject);
        
    }

    private void ChangeWeapon(int slot)
    {
       
        WeaponManager.Instance.SwitchActiveWeapon(slot);

    }

    private void DropAction()
    {
        WeaponManager.Instance.DropCurrentWeapon();
    }
}
