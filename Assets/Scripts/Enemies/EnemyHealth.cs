using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float health = 100f;

    [Header("Canvas")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Canvas canvas;
    [Header("On Hit")]
    [SerializeField] private Color hitColor = new Color(1f, 0.30f, 0.30f);
    [SerializeField] private float timeToColor = 0.3f;

    private Color defaultColor = new Color(1f, 1f, 1f);

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Slider properties
        healthSlider.value = health;
        healthSlider.maxValue = health;
    }

    public void TakeDamage(float damage, Vector2 damageDirection)
    {
        AudioManager.Instance.PlaySFXSound("hit");
        StartCoroutine(SwitchColor());

        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            rb.simulated = false;
            canvas.enabled = false;
            animator.SetTrigger("death");
        }
    }

    // Function called in an animation event
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator SwitchColor()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(timeToColor);
        sr.color = defaultColor;
    }
}
