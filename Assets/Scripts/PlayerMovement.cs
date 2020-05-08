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

    private bool isGrounded=false;
    private Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

   
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Check if player is grounded
        int layerMask = LayerMask.GetMask("Ground");
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        List<Collider2D> results = new List<Collider2D>();
        groundCheck.OverlapCollider(filter, results);
        if (results.Count > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if(isGrounded==true)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            rb2d.AddForce(movement * speed);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
        }
        else if(GetComponent<FireGrapple>().IsWebActive())
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            rb2d.AddForce((movement * speed)/swingControlModifier);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
        }
        else
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            rb2d.AddForce((movement * speed)/airControlModifier);
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
        }

        if (moveVertical != 0)
        {
            GetComponent<FireGrapple>().CallChangePositionOnWeb(moveVertical);
        }
        Debug.Log(results.Count);
    }
}

