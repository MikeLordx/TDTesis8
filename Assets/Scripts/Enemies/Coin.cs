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

    private Rigidbody rb;
    private bool isFloating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = Mathf.Abs(randomDirection.y);
        rb.AddForce(randomDirection * launchForce, ForceMode.Impulse);
        StartCoroutine(StopAndFloat());
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
