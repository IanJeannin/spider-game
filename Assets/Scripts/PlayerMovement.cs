using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxVelocity;

    private Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

   
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        if(moveVertical!=0)
        {
            GetComponent<FireGrapple>().CallChangePositionOnWeb(moveVertical);
        }
        Vector2 movement = new Vector2(moveHorizontal,0);
        
        rb2d.AddForce(movement * speed);
        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxVelocity);
    }
}

