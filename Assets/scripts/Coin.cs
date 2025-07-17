using UnityEngine;

public class Coin : MonoBehaviour
{
    public float lifeTime = 5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.2f;
        rb.angularDamping = 0.5f;

        rb.AddTorque(Random.insideUnitSphere * 5f);

        Destroy(gameObject, lifeTime);
    }
}

