using System.Collections;
using UnityEngine;

public class Enemigovol : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerTarget;
    public GameObject warningIndicatorPrefab;
    public GameObject damageRayPrefab;

    [Header("Comportamiento")]
    public float attackCooldown = 5f;
    public float warningDuration = 1.5f;
    public float hoverHeight = 10f; 
    public float hoverSpeed = 2f;

    private bool canAttack = true;

    void Start()
    {
        if (playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = new Vector3(transform.position.x, hoverHeight, transform.position.z);
    }

    void Update()
    {
        if (playerTarget == null) return;

        transform.LookAt(playerTarget);

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hoverHeight, transform.position.z), Time.deltaTime * hoverSpeed);

        if (canAttack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        canAttack = false;

        Vector3 targetPosition = new Vector3(playerTarget.position.x, 0.01f, playerTarget.position.z); 
        GameObject warningIndicator = Instantiate(warningIndicatorPrefab, targetPosition, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        Destroy(warningIndicator);
        GameObject damageRay = Instantiate(damageRayPrefab, targetPosition, Quaternion.identity);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

