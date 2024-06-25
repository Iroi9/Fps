using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : Interactable
{   
    [SerializeField] float delay = 3f;
    [SerializeField] float areaOfEffect = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countDown;

    bool hasExploded;
    public bool hasBeenThrown;

    public enum ThowableTyp
    {
        None,
        Grenade,
        Smoke
    }

    public ThowableTyp thowableTyp;
    
    public enum ThrowableClass
    {
        None,
        Lethal,
        Tactical
    }
    public ThrowableClass throwableClass;

    private void Start()
    {
        countDown = delay;
    }

    private void Update()
    {

        if (hasBeenThrown == true)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0 && hasExploded == false)
            {
                Explod();
                hasExploded = true;
            }
        }
     
    }

    private void Explod()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (thowableTyp)
        {
            case ThowableTyp.Grenade:
                GrenadeEffect();
                break;
            case ThowableTyp.Smoke:
                SmokeEffect();
                break;
        }
    }

    private void SmokeEffect()
    {
        GameObject smokeEffect = GlobalRef.Instance.smokeEffectGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.smoke);

        Collider[] coliders = Physics.OverlapSphere(transform.position, areaOfEffect);
        foreach (Collider colider in coliders)
        {
            Rigidbody rb = colider.GetComponent<Rigidbody>();
            if (rb != null)
            {
               
            }

           
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosioEffect = GlobalRef.Instance.grenadeExplosionEffect;
        Instantiate(explosioEffect, transform.position,transform.rotation);

        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.explosion);

        Collider[] coliders = Physics.OverlapSphere(transform.position,areaOfEffect);
        foreach (Collider colider in coliders)
        {
            Rigidbody rb = colider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, areaOfEffect);
            }

            //TODO Add damage calculasion
        }
    }

    protected override void Interact(GameObject gameObject)
    {
        WeaponManager.Instance.PickUpThrowable(gameObject);
        

    }


}
