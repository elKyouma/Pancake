using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletDamage = 10f;
    [SerializeField]
    private float bulletLifeTime = 2f;
    [SerializeField]
    private GameObject bulletDestroy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.05f);
        GameObject particleSystem = Instantiate(bulletDestroy, transform.position, Quaternion.identity, null);
        particleSystem.GetComponent<ParticleSystemRenderer>().sortingOrder = collision.contacts[0].normal.y < 0f ? 1 : -1;
        Destroy(particleSystem, 0.5f);

        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        }
    }
    void Start()
    {
        Destroy(gameObject, bulletLifeTime);
    }
}
