using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(gameObject.name + " ha recibido " + amount + " de daño.");

        currentHealth -= amount;
        Debug.Log($"{name} recibió {amount} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{name} ha muerto.");
        GameManager.Instance.EnemyDefeated();
        Destroy(gameObject);
    }
}

