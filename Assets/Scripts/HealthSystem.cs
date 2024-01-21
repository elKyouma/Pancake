using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    private float minHealth = 0;
    [SerializeField]
    private float maxHealth = 100;

    public GameObject healthBar;
    [SerializeField]
    private Vector2 healthBarOffset = new Vector2(0, 1.6f);
    private Vector2 healthBarScale;
    [SerializeField] private Color[] healthBarColors;

    void Start()
    {
        health = maxHealth;
        healthBarScale = healthBar.transform.localScale;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= minHealth)
        {
            health = minHealth;
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    void UpdateHealthBar()
    {
        if (healthBar)
        {
            // healthBar.transform.position = transform.position + (Vector3)healthBarOffset;
            healthBar.transform.localScale = new Vector3(healthBarScale.x * health / maxHealth, healthBarScale.y, 1);
            Color lerpedColor = Color.Lerp(healthBarColors[1], healthBarColors[0], (float)health / maxHealth);
            healthBar.GetComponent<SpriteRenderer>().color = lerpedColor;
        }
    }

    public void Paralyze()
    {
        Debug.Log("Paralyzed");
        GetComponent<Shooting>().enabled = false;
        GetComponentInChildren<Animator>().StopPlayback();
        GetComponentInChildren<Animator>().enabled = false;
        // TODO: add some particle effect, maybe?
        GetComponent<AIPath>().enabled = false; // .maxSpeed = 0;        
    }
}
