using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleCounter : MonoBehaviour
{
    [SerializeField]
    private Text collectiblesDisplay;

    private int collectiblesCollected = 0;
    private int maxCollectibles;

    private void Start()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        maxCollectibles = collectibles.Length;
        Debug.Log(maxCollectibles);
        collectiblesDisplay.text = collectiblesCollected + "/" + maxCollectibles;
    }

    private void Update()
    {
        collectiblesDisplay.text = collectiblesCollected + "/" + maxCollectibles;
    }

    public void AddCollectible()
    {
        collectiblesCollected++;
    }
}
