using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeapon : MonoBehaviour
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

    //Animations
    public GameObject muzzleEffect;
    private Animator animator;


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
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (currShootingMode == ShootingMode.Auto)
        {
            isShooting = playerInput.onFootActions.Shoot.IsPressed();
        }else if (currShootingMode == ShootingMode.Single || currShootingMode == ShootingMode.Burst){
            isShooting = playerInput.onFootActions.Shoot.triggered;
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            fireWeapon();
        }
    }

    private void fireWeapon()
    {

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.shootingSound1911.Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpred().normalized; 

        //Init bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Position bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Fire bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
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
            Invoke("fireWeapon", shootingDelay);
        }
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
        float y = UnityEngine.Random.Range(spreadIntensity, spreadIntensity);

        return direction + new Vector3(x,y,0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletLife)
    {
        yield return new WaitForSeconds(bulletLife);
        Destroy(bullet);
    }
}
