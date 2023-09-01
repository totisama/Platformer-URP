using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;
    [Header("On Hit")]
    [SerializeField] private float timeToMove = 1f;
    [SerializeField] private Color hitColor = new Color(1f, 0.30f, 0.30f);
    [SerializeField] private float timeToColor = 0.2f;
    [Header("Extra health potion")]
    [SerializeField] private GameObject extraHealth;
    [SerializeField] private Slider extraHealthSlider;


    private float health;
    private bool extraHealthActive;
    private Color defaultColor = new Color(1f, 1f, 1f);

    private Animator animator;
    private PlayerController playerController;
    private PlayerSpawn playerSpawn;
    private TakeKnockBack takeKnockBack;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float Health
    {
        get { return health; }
        private set { health = value; }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerSpawn = GetComponent<PlayerSpawn>();
        takeKnockBack = GetComponent<TakeKnockBack>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InitializeHealth();
    }

    private void InitializeHealth()
    {
        health = maxHealth;
        healthSlider.value = maxHealth;

        extraHealth.SetActive(false);
        extraHealthSlider.value = 0;
    }

    private void ManageExtraHealth(float temporalHealth)
    {
        float currentHealth = temporalHealth - maxHealth;

        if (currentHealth > 0)
        {
            extraHealthSlider.value = currentHealth;
        }
        else
        {
            extraHealthSlider.value = 0;
            extraHealth.SetActive(false);
            extraHealthActive = false;
            healthSlider.value = temporalHealth;
        }
    }

    public void TakeDamage(float damage, Vector2 damageDirection, Vector2 multiplier)
    {
        float temporalHealth = Health - damage;

        if (extraHealthActive && Health > maxHealth)
        {
            ManageExtraHealth(temporalHealth);
        }
        else
        {
            healthSlider.value = temporalHealth;
        }

        takeKnockBack.KnockBack(damageDirection, multiplier);
        Health = temporalHealth;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(SwitchColor());
            StartCoroutine(ImmobilizePlayer());
        }
    }

    public void TakeDamage(float damage, Vector2 damageDirection)
    {
        float temporalHealth = Health - damage;

        if (extraHealthActive && Health > maxHealth)
        {
            ManageExtraHealth(temporalHealth);
        }
        else
        {
            healthSlider.value = temporalHealth;
        }

        takeKnockBack.KnockBack(damageDirection, new Vector2(1.0f, 1.0f));
        Health = temporalHealth;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(SwitchColor());
            StartCoroutine(ImmobilizePlayer());
        }
    }

    private void Die()
    {
        playerController.canMove = false;
        rb.simulated = false;
        animator.SetTrigger("death");
    }

    private void IncreaseHealth(int amount)
    {
        float temporalHealth = Health + amount;
        bool greaterThanMax = temporalHealth > maxHealth;

        // Only activate the slider if is necessary
        if (extraHealthActive && greaterThanMax)
        {
            extraHealthSlider.value = temporalHealth - maxHealth;
            healthSlider.value = maxHealth;
            extraHealth.SetActive(true);
        }
        else if (greaterThanMax)
        {
            temporalHealth = maxHealth;
            healthSlider.value = maxHealth;
            extraHealthActive = false;
        }
        else
        {
            healthSlider.value = temporalHealth;
            extraHealthActive = false;
        }

        Health = temporalHealth;
    }

    internal bool UseExtraHealthPotion(int amount)
    {
        if (extraHealthActive)
        {
            return false;
        }

        extraHealthActive = true;

        IncreaseHealth(amount);
        return true;
    }

    public void SetHealthObjects(GameObject newExtraHealth, Slider newHealthSlider, Slider newExtraHealthSlider)
    {
        extraHealth = newExtraHealth;

        newHealthSlider.value = maxHealth;
        healthSlider = newHealthSlider;

        extraHealthSlider = newExtraHealthSlider;
    }

    private void RespawnPlayer()
    {
        rb.velocity = Vector2.zero;
        InitializeHealth();
        playerController.UnlockPlayer();
        GameManager.Instance.RespawnPlayer(playerSpawn.spawnPosition);
    }

    private IEnumerator ImmobilizePlayer()
    {
        playerController.canMove = false;
        yield return new WaitForSeconds(timeToMove);
        playerController.canMove = true;
    }

    private IEnumerator SwitchColor()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(timeToColor);
        sr.color = defaultColor;
    }
}
