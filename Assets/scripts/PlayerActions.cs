using UnityEngine;
using TMPro;

public class PlayerActions : MonoBehaviour
{
    [Header("Referencias")]
    public Camera playerCamera;
    public Transform coinSpawnPoint;
    public GameObject coinPrefab;
    public Transform parryZone;
    public UIManager uiManager;
    public ParticleSystem muzzleFlash; 

    [Header("Disparo con Proyectil")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 5f; 
    private float nextFireTime = 0f;

    [Header("Estadísticas del Jugador")]
    public int maxHealth = 100;
    private int currentHealth;
    public int maxCoins = 3;
    private int currentCoins;

    [Header("Fuerza de la Moneda")]
    public float coinUpwardForce = 4f;
    public float coinForwardForce = 5f;

    [Header("Parry")]
    public float parryDuration = 0.2f;
    private bool isParrying = false;
    private float parryCooldownTimer = 0f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentCoins = maxCoins;
        if (uiManager != null)
        {
            uiManager.UpdateHealthBar(currentHealth, maxHealth);
            uiManager.UpdateCoinCount(currentCoins);
        }
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TossCoin();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            StartParry();
        }

        if (parryCooldownTimer > 0)
        {
            parryCooldownTimer -= Time.deltaTime;
        }

        parryZone.gameObject.SetActive(isParrying);
    }

    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void TossCoin()
    {
        if (currentCoins > 0)
        {
            GameObject coinGO = Instantiate(coinPrefab, coinSpawnPoint.position, playerCamera.transform.rotation);
            Rigidbody coinRb = coinGO.GetComponent<Rigidbody>();
            if (coinRb != null)
            {
                Vector3 forceDirection = playerCamera.transform.forward * coinForwardForce + playerCamera.transform.up * coinUpwardForce;
                coinRb.AddForce(forceDirection, ForceMode.Impulse);
            }
            currentCoins--;
            if (uiManager != null) uiManager.UpdateCoinCount(currentCoins);
        }
    }

    void StartParry()
    {
        if (parryCooldownTimer <= 0)
        {
            isParrying = true;
            Invoke(nameof(StopParry), parryDuration);
        }
    }

    void StopParry()
    {
        isParrying = false;
        parryCooldownTimer = 0.5f;
    }

    public bool IsPlayerParrying()
    {
        return isParrying;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (uiManager != null) uiManager.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreCoin()
    {
        if (currentCoins < maxCoins)
        {
            currentCoins++;
            if (uiManager != null) uiManager.UpdateCoinCount(currentCoins);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("El jugador ha muerto.");
        GameManager.Instance.PlayerDied();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<MouseLook>().enabled = false;
        this.enabled = false;
    }
}




