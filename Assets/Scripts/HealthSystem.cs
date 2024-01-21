using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Animator))]
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
    [SerializeField] private Color paralyzeColor;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        health = maxHealth;

        if (healthBar)
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
        animator.SetTrigger("Die");
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
        Destroy(GetComponent<AIPath>());
        Destroy(GetComponent<AIDestinationSetter>());
        Destroy(GetComponent<Shooting>());
        GetComponentInChildren<Animator>().StopPlayback();
        GetComponentInChildren<Animator>().enabled = false;
        // TODO: add some particle effect, maybe?
        GetComponent<AIPath>().enabled = false; // .maxSpeed = 0;   \
        GetComponent<Rigidbody2D>().simulated = false; // .velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().color = paralyzeColor;
    }
}
