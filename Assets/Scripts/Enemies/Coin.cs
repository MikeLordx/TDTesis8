using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public int coinValue = 10;
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float floatHeight = 0.5f;
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float stopTime = 1f;

    [SerializeField] private float lifeTime = 10f;
    [SerializeField][Range(0f, 1f)] private float spawnProbability = 0.5f;

    public AudioClip coinSound;

    private Rigidbody rb;
    private bool isFloating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (Random.value > spawnProbability)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = Mathf.Abs(randomDirection.y);
        rb.AddForce(randomDirection * launchForce, ForceMode.Impulse);
        StartCoroutine(StopAndFloat());
        StartCoroutine(DestroyAfterLifetime());
    }

    void Update()
    {
        if (isFloating)
        {
            Vector3 position = transform.position;
            position.y += Mathf.Sin(Time.time * floatSpeed) * Time.deltaTime;
            transform.position = position;
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator StopAndFloat()
    {
        yield return new WaitForSeconds(stopTime);
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        isFloating = true;
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCharacter"))
        {
            GameManager.instance.AddCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(coinSound);
        }
    }
}
