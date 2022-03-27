using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    [SerializeField] int points = 100;
    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(points);
            Destroy(gameObject);
        }
    }
}
