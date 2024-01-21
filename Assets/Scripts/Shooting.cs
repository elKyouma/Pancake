using System.Collections;
using System.Collections.Generic;
using Pathfinding;
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
    private AIDestinationSetter DestinationSetter;
    private float originalRange;
    private bool originalRangePlayerSeen;
    [SerializeField]
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;


    [SerializeField]
    private bool isPeaceful = true;
    public void MakeAngry()
    {
        isPeaceful = false;
        GetComponent<AIPath>().endReachedDistance = originalRange;
        GetComponent<AIPath>().butMustSeePlayer = originalRangePlayerSeen;
        DestinationSetter.target = Player.transform;

    }

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
        var aipath = GetComponent<AIPath>();
        originalRange = aipath.endReachedDistance;
        aipath.endReachedDistance = 1;
        originalRangePlayerSeen = aipath.butMustSeePlayer;
        aipath.butMustSeePlayer = false;

        Player = GameObject.FindGameObjectWithTag("Player");
        DestinationSetter = GetComponent<AIDestinationSetter>();
        if (Bullet == null) Debug.LogError("Shooting: Bullet is null");
        bulletRadius = Bullet.transform.localScale.x;
        currentAmmo = maxAmmo;
        StartCoroutine(Shoot());
        StartCoroutine(ChangePOI());
    }

    void Update()
    {
        if (IsPlayerInRange()) UpdateBulletSpawnPosition();
        WeaponRotate();
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


    IEnumerator ChangePOI()
    {
        if (!isPeaceful)
        {
            DestinationSetter.target = Player.transform;
            yield break;
        }
        var pois = GameObject.FindGameObjectsWithTag("POI");
        var poi = pois[Random.Range(0, pois.Length)];

        DestinationSetter.target = poi.transform;
        yield return new WaitForSeconds(20);
        StartCoroutine(ChangePOI());

    }

    IEnumerator Shoot()
    {
        if (isPeaceful)
        {
            yield return new WaitForSeconds(1f);
        }
        else if (IsPlayerInRange())
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
        }
        else
        {
            if (currentAmmo < maxAmmo / 2)
            {
                yield return new WaitForSeconds(reloadTime);
                currentAmmo = maxAmmo;
            }
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(Shoot());
    }
    void UpdateBulletSpawnPosition()
    {
        var dist = WeaponPlaceholder.transform.position - Player.transform.position;
        float degree = 180 - Vector2.SignedAngle(dist, Vector2.right);
        if (!isPeaceful)
        {
            WeaponPlaceholder.transform.rotation = Quaternion.Euler(0, 0, degree);
            // if (degree > 180)
            // {
            //     WeaponPlaceholder.transform.localScale = new Vector3(1, -1, 1);
            // }
            // else
            // {
            //     WeaponPlaceholder.transform.localScale = new Vector3(1, 1, 1);
            // }
        }
    }
    public void WeaponRotate()
    {
        Vector2 dir = GetComponent<AIPath>().velocity2D;
        if (!isPeaceful && IsPlayerInRange())
            dir = Player.transform.position - transform.position;
        if (dir == Vector2.zero) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponPlaceholder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (angle > 90 || angle < -90)
        {
            WeaponPlaceholder.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            WeaponPlaceholder.transform.localScale = new Vector3(1, -1, 1);
        }
    }
}