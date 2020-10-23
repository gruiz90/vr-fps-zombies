using UnityEngine;
using VRTK.Examples;
using UnityEngine.UI;
using UnityEditor;
using System;
using VRTK; //VRTK_ControllerEvents

public class Handgun : GunShoot
{
    public Animator anim;
    public GameObject flashPrefab;
    public AudioClip shootSound;

    private int ammo;
    private int magazineSize = 10;
    private bool reloading;

    public Text ammoText;
    public VRTK_ControllerEvents events;
    private void Start()
    {
        ammo = magazineSize;
        ammoText.text = ammo.ToString();
        reloading = false;

        events = GameObject.FindObjectOfType<VRTK_ControllerEvents>(); 
    }

    private void Update()
    {
        if (events.buttonOnePressed && !reloading)
        {
            Reload();
        }
    }

    protected override void FireProjectile()
    {
        if (ammo > 0 && !reloading)
        {
            base.FireProjectile();
            anim.SetTrigger("Fire");

            var muzzleFlash = Instantiate(flashPrefab,
                projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            Destroy(muzzleFlash, 0.3f);

            AudioSource.PlayClipAtPoint(shootSound, transform.position);

            ammo--;
            ammoText.text = ammo.ToString();

            if (ammo == 0)
            {
                Reload();
            }
        }
    }

    private void Reload()
    {
        ammoText.text = "Reloading...";
        reloading = true;
        Invoke("FinishReload", 1.0f);
    }

    void FinishReload()
    {
        ammo = magazineSize;
        ammoText.text = ammo.ToString();
        reloading = false;
    }
}