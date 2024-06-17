using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRef : MonoBehaviour
{   
    public static GlobalRef Instance {  get; set; }

    public GameObject bulletImpactEffectPrefab;

    public void Awake()
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
}