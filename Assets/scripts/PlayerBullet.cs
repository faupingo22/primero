using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Configuración")]
    public float speed = 50f;
    public float lifeTime = 3f;
    public float damage = 25f;

    [Header("Ricochet")]
    public float ricochetDamageMultiplier = 3f;
    public GameObject ricochetEffectPrefab; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            HandleRicochet(other.transform.position);

            Destroy(other.gameObject);
            Destroy(gameObject);
            return; 
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject); 
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("ParryZone"))
        {
            Destroy(gameObject);
        }
    }

    void HandleRicochet(Vector3 coinPosition)
    {
        if (ricochetEffectPrefab != null)
        {
            Instantiate(ricochetEffectPrefab, coinPosition, Quaternion.identity);
        }

        EnemyHealth closestEnemy = FindClosestEnemy(coinPosition);
        if (closestEnemy != null)
        {
            Debug.Log("¡Rebote exitoso contra " + closestEnemy.name + "!");
            float ricochetDamage = damage * ricochetDamageMultiplier;
            closestEnemy.TakeDamage(ricochetDamage);
        }
    }

    EnemyHealth FindClosestEnemy(Vector3 fromPosition)
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        EnemyHealth closest = null;
        float minDistance = Mathf.Infinity;

        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector3.Distance(fromPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }
}

