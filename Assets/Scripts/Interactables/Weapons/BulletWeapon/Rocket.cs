using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{

    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;
    protected override void CreateEffectOnInpact(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject explosion = Instantiate(
            GlobalRef.Instance.grenadeExplosionEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        SoundManager.Instance.shootingChannel.PlayOneShot(SoundManager.Instance.explosion);

        Collider[] coliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider colider in coliders)
        {
            Rigidbody rb = colider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            //TODO Add damage calculasion
        }

    }
}
