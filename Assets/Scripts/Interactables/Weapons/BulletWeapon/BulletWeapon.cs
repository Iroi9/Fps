using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : Interactable
{
     

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    
    private InputManager playerInput;
    //Bullet
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float bulletVelocity = 100;
    [SerializeField] float bulletLife = 3f;
    [SerializeField] bool isReloading;

    //Reload
    [SerializeField] float reloadTime;
    [SerializeField] int magazineSize, bulletsLeft;
    private int maxAmmo;
    //Animations
    public GameObject muzzleEffect;
    private Animator animator;

    //Pos on Player
    public Vector3 spawnPos;
    public Vector3 spawnRot;
    public Vector3 spawnScale;

    public bool isActiveWeapon;
    public enum WeaponModel
    {
        Pistol1911,
        AK47
    }

    public WeaponModel weaponModel;
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
        else if (playerInput.onFootActions.SwitchWepSlot1.IsPressed())
        {
            ChangeWeapon(0);
        }
        else if(playerInput.onFootActions.SwitchWepSlot2.IsPressed())
        {
            ChangeWeapon(1);
        }
        else
        {
            if (isActiveWeapon)
            {
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

                if (AmmoManager.Instance.ammoDisplay != null)
                {
                    AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{maxAmmo / bulletsPerBurst}";
                }
            }
        }
       
       
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
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
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        
        //BurstMode
        if (currShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
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

    protected override void Interact(Interactable gameObject)
    {
            WeaponManager.Instance.PickUpWeapon(gameObject);
        
    }

    private void ChangeWeapon(int slot)
    {
            WeaponManager.Instance.SwitchActiveWeapon(slot);
        
        
    }
}
