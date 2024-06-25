using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            CreateEffectOnInpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            CreateEffectOnInpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            CreateEffectOnInpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            CreateEffectOnInpact(collision);

            Destroy(gameObject);
        }
    }
    protected virtual void CreateEffectOnInpact(Collision collision)
    {

    }
}
