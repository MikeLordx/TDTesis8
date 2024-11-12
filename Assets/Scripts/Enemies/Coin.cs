using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public int coinValue = 10;
    private Rigidbody rb;
    private int bounceCount = 0;
    private int maxBounces = 3;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("groundLayer"))
        {
            bounceCount++;
            if (bounceCount >= maxBounces)
            {
                StopCoin();
            }
        }
    }

    private void StopCoin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
