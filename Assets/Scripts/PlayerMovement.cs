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
    private float jumpHeight;

    private bool isGrounded=false;
    private bool isSliding = false;
    private Rigidbody2D rb2d;
    private float moveHorizontal;
    private float moveVertical;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
        groundCheck.OverlapCollider(filter, results);
        
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
        if(isGrounded==true&&Input.GetKeyDown(KeyCode.Space))
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
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.AddForce(movement * speed);
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
            }
            else if (GetComponent<FireGrapple>().IsWebActive()) //If the player is not grounded but attached to the web
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.AddForce((movement * speed) / swingControlModifier);
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
            }
            else //If the player is in midair
            {
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
}

