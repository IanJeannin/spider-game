using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrapple : MonoBehaviour
{
    [SerializeField]
    private GameObject grapplePrefab;
    [SerializeField]
    private float speedOfGrapple;
    [SerializeField]
    private float maxDistanceOfGrapple;

    private GameObject currentGrapple;
    private GameObject player;
    private RaycastHit2D raycastHit;
    private bool isWebActive;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isWebActive == false)
            {
                int layerMask = LayerMask.GetMask("Ground");
                Vector2 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                raycastHit = Physics2D.Raycast(player.transform.position, clickedPosition - (Vector2)player.transform.position, maxDistanceOfGrapple,layerMask);
                if(raycastHit.collider!=null)
                {
                    Vector2 endPosition = raycastHit.point;
                    currentGrapple = Instantiate(grapplePrefab, endPosition, Quaternion.identity);
                    currentGrapple.GetComponent<Rigidbody2D>().AddForce(endPosition * speedOfGrapple);
                    currentGrapple.GetComponent<Web>().SetEndPosition(endPosition);
                    isWebActive = true;
                }
            }
            else
            {
                Destroy(currentGrapple);
                isWebActive = false;
            }
        }
    }

    public void CallChangePositionOnWeb(float verticalAxis)
    {
        if(currentGrapple!=null)
        {
            currentGrapple.GetComponent<Web>().ChangePositionOnWeb(verticalAxis);
        }
    }

    public float GetMaxGrappleDistance()
    {
        return maxDistanceOfGrapple;
    }

    public bool IsWebActive()
    {
        return isWebActive;
    }
}
