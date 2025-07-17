using UnityEngine;
using UnityEngine.AI; 

[RequireComponent(typeof(NavMeshAgent))]
public class KamikazeAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerTarget;
    public GameObject explosionEffectPrefab; 

    [Header("Estadísticas")]
    public float explosionRadius = 5f;
    public int explosionDamage = 50;
    public float detectionRange = 2f; 

    private NavMeshAgent agent;
    private bool hasExploded = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (playerTarget == null || hasExploded) return;

        agent.SetDestination(playerTarget.position);

        if (Vector3.Distance(transform.position, playerTarget.position) <= detectionRange)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log(name + " ha explotado!");

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            PlayerActions player = hit.GetComponent<PlayerActions>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }

            EnemyHealth otherEnemy = hit.GetComponent<EnemyHealth>();
            if (otherEnemy != null && otherEnemy.gameObject != this.gameObject)
            {
                otherEnemy.TakeDamage(explosionDamage);
            }
        }

        GameManager.Instance.EnemyDefeated();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

