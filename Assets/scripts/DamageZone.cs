using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 30;
    public float activeDuration = 0.5f; 

    void Start()
    {
        Destroy(gameObject, activeDuration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>()?.TakeDamage(damage);
        }
    }
}