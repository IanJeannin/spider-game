using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private CircleCollider2D groundCheck;
    [Tooltip("The number that speed will be divided by while in the air")]
    [SerializeField]
    private float airControlModifier;
    [SerializeField]
    private float swingControlModifier;
    [SerializeField]
    private float massOnWeb;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private bool isJumpEnabled=false;
    [SerializeField]
    private float groundCheckDistance=1f;

    private bool isGrounded=false;
    private bool isSliding = false;
    private Rigidbody2D rb2d;
    private float moveHorizontal;
    private float moveVertical;
    private int collectibles = 0;
    private float massOffWeb;
    private RaycastHit2D groundCheckRaycast;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        massOffWeb = rb2d.mass;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        //Check if player is grounded
        int layerMask = LayerMask.GetMask("Ground","Water");
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        List<Collider2D> results = new List<Collider2D>();
        groundCheckRaycast = Physics2D.Raycast(transform.position, Vector2.down,groundCheckDistance,layerMask);
        if (groundCheckRaycast.transform != null)
        {
            if (groundCheckRaycast.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = true;
                isSliding = false;
            }
            else if (groundCheckRaycast.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                isSliding = true;
                isGrounded = false;
            }
            
            Debug.Log(isGrounded);
        }
        else
        {
            isGrounded = false;
            isSliding = false;
        }
        /*groundCheck.OverlapCollider(filter, results);
        
        if (results.Count > 0)
        {
            if (results[0].gameObject.layer == 4) //Check if it overlaps the "Water" layer
            {
                isSliding = true; //If so, disable movement
            }
            else
            {
                isGrounded = true;
                isSliding = false;
            }
        }
        else
        {
            isGrounded = false;
            isSliding = false;
        }
        */
        if (isGrounded==true&&Input.GetKeyDown(KeyCode.Space)&&isJumpEnabled)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isSliding == false)
        {
            if (isGrounded == true)
            {
                rb2d.freezeRotation = true;
                rb2d.rotation = 0;
                rb2d.mass = massOffWeb;
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.AddForce(movement * speed);
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
            }
            else if (GetComponent<FireGrapple>().IsWebActive()) //If the player is not grounded but attached to the web
            {
                rb2d.freezeRotation = false;
                rb2d.mass = massOnWeb;
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.AddForce((movement * speed) / swingControlModifier);
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
            }
            else //If the player is in midair
            {
                //rb2d.freezeRotation = true;
                //rb2d.rotation = 0;
                rb2d.mass = massOffWeb;
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.AddForce((movement * speed) / airControlModifier);
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
            }

            if (moveVertical != 0)
            {
                GetComponent<FireGrapple>().CallChangePositionOnWeb(moveVertical);
            }
        }
    }

    private void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
    }
    
    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void AddCollectible()
    {
        collectibles++;
        Debug.Log("Collectibles Collected: " + collectibles);
    }
}

