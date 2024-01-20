using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletDamage = 10f;
    [SerializeField]
    private float bulletLifeTime = 2f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.05f);
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
