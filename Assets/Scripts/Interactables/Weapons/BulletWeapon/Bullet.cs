using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{

    protected override void CreateEffectOnInpact(Collision collision)
    {
            ContactPoint contact = collision.contacts[0];

            GameObject hole = Instantiate(
                GlobalRef.Instance.bulletImpactEffectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );
            hole.transform.SetParent(collision.gameObject.transform);
              
    }
}
