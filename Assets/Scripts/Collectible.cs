using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameObject collectibleCounter;

    private void Start()
    {
        collectibleCounter = GameObject.FindGameObjectWithTag("CollectibleCounter");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            collectibleCounter.GetComponent<CollectibleCounter>().AddCollectible();
            Destroy(gameObject);
        }
    }
}
