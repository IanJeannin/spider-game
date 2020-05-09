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
    [Tooltip("The max amount of webs able to be shot before touching the ground again.")]
    [SerializeField]
    private int maxWebs=2;

    private GameObject currentGrapple;
    private GameObject player;
    private RaycastHit2D raycastHit;
    private RaycastHit2D maxDistanceRaycast;
    private bool isWebActive;
    private LineRenderer lineRenderer;
    private float currentNumberOfWebs = 0;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        bool isGrounded = GetComponent<PlayerMovement>().GetIsGrounded();
        if(isGrounded==true)
        {
            currentNumberOfWebs = 0;
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (isWebActive == false&&currentNumberOfWebs<maxWebs)
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
                    currentNumberOfWebs++;
                }
            }
            else
            {
                Destroy(currentGrapple);
                isWebActive = false;
            }
        }
        if(isWebActive==true&&Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(currentGrapple);
            isWebActive = false;
        }
        maxDistanceRaycast = Physics2D.Raycast(player.transform.position, Input.mousePosition, maxDistanceOfGrapple);

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
