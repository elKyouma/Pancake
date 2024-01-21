using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private GameObject Player;
    [SerializeField, Tooltip("default is Prefabs\\Bullet")]
    private GameObject Bullet;
    [SerializeField]
    private GameObject Graphic;
    [SerializeField]
    private GameObject BulletSpawn;
    [SerializeField]
    private GameObject WeaponPlaceholder;
    [SerializeField]
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;

    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float visionRange = 10f;
    [SerializeField]
    private float maxAngleAccuracyOffset = 0f; // how much the enemy can miss the player by (in degrees)  

    [SerializeField]
    private int maxAmmo = 6;
    private int currentAmmo;
    [SerializeField]
    private float reloadTime = 1f;
    private float bulletRadius;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Bullet == null) Debug.LogError("Shooting: Bullet is null");
        bulletRadius = Bullet.transform.localScale.x;
        currentAmmo = maxAmmo;
        StartCoroutine(Shoot());
    }

    void Update()
    {
        if (IsPlayerInRange()) UpdateBulletSpawnPosition();
    }

    bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer > visionRange) return false;
        return RaycastHelper.IsInBulletSight(Player.transform.position, transform.position, bulletRadius);
    }

    void OnDrawGizmos()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
        Gizmos.DrawLine(Player.transform.position, transform.position);
    }

    IEnumerator Shoot()
    {
        if (IsPlayerInRange())
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
            if (currentAmmo < maxAmmo / 2)
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
        var dist = WeaponPlaceholder.transform.position - Player.transform.position;
        float degree = 180 - Vector2.SignedAngle(dist, Vector2.right);
        WeaponPlaceholder.transform.rotation = Quaternion.Euler(0, 0, degree);
    }
}
