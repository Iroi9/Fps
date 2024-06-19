using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletWeapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
   
    public AudioSource shootingChannel;
    public AudioSource emptySound1911;

    public AudioSource reloadingSound1911;
    public AudioClip m1911shot;

    public AudioSource reloadingSoundAK47;
    public AudioClip ak47Shot;
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

    public void PlayShootingSound(WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case WeaponModel.Pistol1911:
                shootingChannel.PlayOneShot(m1911shot);
                break;
            case WeaponModel.AK47:
                shootingChannel.PlayOneShot(ak47Shot);
                break;
        }
    }
    public void PlayReloadingSound(WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case WeaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case WeaponModel.AK47:
                reloadingSoundAK47.Play();
                break;
        }
    }
}
