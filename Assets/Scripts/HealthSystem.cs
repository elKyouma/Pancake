using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int health;
    private int minHealth = 0;
    [SerializeField]
    private int maxHealth = 100;

    public GameObject healthBar;
    [SerializeField]
    private Vector2 healthBarOffset = new Vector2(0, 1f);
    private Vector2 healthBarScale;
    [SerializeField] private Color[] healthBarColors;

    void Start()
    {
        health = maxHealth;
        healthBarScale = healthBar.transform.localScale;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
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
            healthBar.transform.position = transform.position + (Vector3)healthBarOffset;
            healthBar.transform.localScale = new Vector3(healthBarScale.x * health / maxHealth, healthBarScale.y, 1);
            Color lerpedColor = Color.Lerp(healthBarColors[1], healthBarColors[0], (float)health / maxHealth);
            healthBar.GetComponent<SpriteRenderer>().color = lerpedColor;
        }
    }
}