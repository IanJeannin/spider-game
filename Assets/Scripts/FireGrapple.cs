using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrapple : MonoBehaviour
{
    [SerializeField]
    private GameObject grapplePrefab;

    private GameObject currentGrapple;
    private bool isWebActive;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isWebActive == false)
            {
                Vector2 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentGrapple = Instantiate(grapplePrefab, transform.position, Quaternion.identity);
                currentGrapple.GetComponent<Web>().SetEndPosition(clickedPosition);
                isWebActive = true;
            }
            else
            {
                Destroy(currentGrapple);
                isWebActive = false;
            }
        }
    }
}
