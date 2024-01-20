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
    private float bulletLifeTime = 2f;
    [SerializeField]
    private float visionRange = 10f;
    [SerializeField]
    private float maxAngleAccuracyOffset = 0f; // how much the enemy can miss the player by (in degrees)  

    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) < visionRange)
        {
            yield return new WaitForSeconds(fireRate);
            GameObject bullet = Instantiate(Bullet, BulletSpawn.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = ((Player.transform.position - transform.position).normalized + new Vector3(Random.Range(-maxAngleAccuracyOffset, maxAngleAccuracyOffset), Random.Range(-maxAngleAccuracyOffset, maxAngleAccuracyOffset), 0)) * bulletSpeed;
            Destroy(bullet, bulletLifeTime);
            StartCoroutine(Shoot());
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Shoot());
        }
    }
}
