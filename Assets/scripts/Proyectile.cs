using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float lifeTime = 5f;
    private GameObject owner;
    private bool hasBeenParried = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    public void SetOwner(GameObject ownerGO)
    {
        owner = ownerGO;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasBeenParried)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyHealth>()?.TakeDamage(damage * 2);
                Destroy(gameObject);
            }
            return;
        }

        if (other.CompareTag("ParryZone"))
        {
            PlayerActions parryPlayer = other.GetComponentInParent<PlayerActions>();
            if (parryPlayer != null && parryPlayer.IsPlayerParrying())
            {
                Parry(parryPlayer);
                return; 
            }
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void Parry(PlayerActions player)
    {
        Debug.Log("¡Parry Exitoso!");
        hasBeenParried = true;
        tag = "PlayerProjectile"; 

        player.RestoreCoin();

        if (owner != null)
        {
            Vector3 directionToOwner = (owner.transform.position - transform.position).normalized;
            rb.linearVelocity = directionToOwner * speed * 1.5f;
        }
        else 
        {
            rb.linearVelocity = player.playerCamera.transform.forward * speed * 1.5f;
        }
    }
}



