using UnityEngine;
using UnityEngine.AI;

public class TempEnemy : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] public int rewardAmount = 10;
    [SerializeField] public GameObject coinPrefab;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0f);
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Die");

        DropCoins();
        GameManager.instance.AddCoins(rewardAmount);
        Destroy(gameObject, 2f);
    }

    void DropCoins()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            rb.AddForce(randomDirection * 5f, ForceMode.Impulse);
            Vector3 randomTorque = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            rb.AddTorque(randomTorque, ForceMode.Impulse);
            rb.drag = 1f;
            rb.angularDrag = 0.5f;
        }
    }
}
