using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Text webCounter;

    private GameObject currentGrapple;
    private GameObject player;
    private RaycastHit2D raycastHit;
    private RaycastHit2D maxDistanceRaycast;
    private bool isWebActive;
    private LineRenderer lineRenderer;
    private float currentNumberOfWebs = 0;
    private LayerMask layerMask;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
        layerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        float counter = maxWebs - currentNumberOfWebs;
        webCounter.text = counter.ToString();
        bool isGrounded = GetComponent<PlayerMovement>().GetIsGrounded();
        if(isGrounded==true)
        {
            currentNumberOfWebs = 0;
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (isWebActive == false&&currentNumberOfWebs<maxWebs)
            {
                Vector2 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                raycastHit = Physics2D.Raycast(player.transform.position, clickedPosition - (Vector2)player.transform.position, maxDistanceOfGrapple,layerMask);
                if(raycastHit.collider!=null)
                {
                    Vector2 endPosition = raycastHit.point;
                    currentGrapple = Instantiate(grapplePrefab, endPosition, Quaternion.identity);
                    currentGrapple.GetComponent<Rigidbody2D>().AddForce(endPosition * speedOfGrapple);
                    currentGrapple.GetComponent<Web>().SetEndPosition(endPosition);
                    isWebActive = true;
                    currentGrapple.transform.SetParent(raycastHit.collider.transform);
                    currentNumberOfWebs++;
                }
            }
            else
            {
                Destroy(currentGrapple);
                currentGrapple.transform.SetParent(null);
                isWebActive = false;
            }
        }
        if(isWebActive==true&&Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(currentGrapple);
            currentGrapple.transform.SetParent(null);
            isWebActive = false;
        }

        lineRenderer.positionCount = 2;
        maxDistanceRaycast = Physics2D.Raycast(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)-player.transform.position, maxDistanceOfGrapple,layerMask);
        Vector2 origin = new Vector3(0, 0);
        if (maxDistanceRaycast.point!=origin)
        {
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, maxDistanceRaycast.point);
        }
        else
        {
            Vector3 mousePos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position);
            mousePos=Vector3.Normalize(mousePos);
            Vector3 maxGrapple = player.transform.position+mousePos * maxDistanceOfGrapple;
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, maxGrapple);
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
