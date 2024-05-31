using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    public float recoilAmount = 0.1f;
    public float recoilRotationAmount = 10f;
    public float recoilRecoverySpeed = 2f;
    public float ClipLength = 1f;



    public List<ParticleSystem> muzzleFlashes;
    public GameObject crosshairRef;
    public int poolSize = 10;

    private AudioSource shootSound;
    private float nextFireTime = 0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private List<GameObject> bulletPool;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
        
        shootSound = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        RecoverFromRecoil();
    }

    void Shoot()
    {
        foreach (ParticleSystem muzzleFlash in muzzleFlashes)
        {
            muzzleFlash.Play();
        }

        crosshairRef.transform.DOPunchScale(new Vector3(1.125f, 1.125f, 1.125f), .25f, 2, .5f);

        GameObject bullet = GetPooledBullet();
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = bulletSpawnPoint.rotation;
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;

            ApplyRecoil();
            shootSound.Play();
        }

       



    }

    GameObject GetPooledBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    void ApplyRecoil()
    {
        transform.localPosition -= new Vector3(0, recoilAmount, recoilAmount / 2);

        float recoilTilt = Random.Range(-recoilRotationAmount, recoilRotationAmount);
        transform.localRotation *= Quaternion.Euler(-recoilRotationAmount, recoilTilt, 0);
    }

    void RecoverFromRecoil()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * recoilRecoverySpeed);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, Time.deltaTime * recoilRecoverySpeed);
    }
}
