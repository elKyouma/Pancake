using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private GameObject BulletSpawn;
    [SerializeField]
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;

    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float visionRange = 10f;
    [SerializeField]
    private float maxAngleAccuracyOffset = 0f; // how much the enemy can miss the player by (in degrees)  

    private int maxAmmo = 6;
    private int currentAmmo;
    private float reloadTime = 1f;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        UpdateBulletSpawnPosition();
        if (Vector2.Distance(transform.position, Player.transform.position) < visionRange)
        {
            if (currentAmmo == 0)
            {
                yield return new WaitForSeconds(reloadTime);
                currentAmmo = maxAmmo;
            }
            yield return new WaitForSeconds(fireRate);
            GameObject bullet = Instantiate(Bullet, BulletSpawn.transform.position, Quaternion.identity);
            currentAmmo--;
            bullet.GetComponent<Rigidbody2D>().velocity = ((Player.transform.position - transform.position).normalized + new Vector3(Random.Range(-maxAngleAccuracyOffset, maxAngleAccuracyOffset), Random.Range(-maxAngleAccuracyOffset, maxAngleAccuracyOffset), 0)) * bulletSpeed;
            StartCoroutine(Shoot());
        }
        else
        {
            if (currentAmmo < maxAmmo)
            {
                yield return new WaitForSeconds(reloadTime);
                currentAmmo = maxAmmo;
            }
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Shoot());
        }
    }

    void UpdateBulletSpawnPosition()
    {
        BulletSpawn.transform.position = transform.position + (Player.transform.position - transform.position).normalized;
    }
}
