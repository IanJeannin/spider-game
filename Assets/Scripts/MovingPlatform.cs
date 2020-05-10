using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    private Transform[] positions;
    [SerializeField]
    private float speed;

    private Vector2 nextPos;
    private int positionsIndex=0;
    private Rigidbody2D rb2d;

    private void Start()
    {
        nextPos = positions[0].position;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = positions[positionsIndex].position - transform.position;
        direction = Vector3.Normalize(direction);
        rb2d.velocity=direction*speed;
        Debug.Log(positionsIndex);
    }

    private void OnDrawGizmos()
    {
        for(int x=1;x<positions.Length;x++)
        {
            Gizmos.DrawLine(positions[x].position, positions[x-1].position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLLIDED");
        if(collision.gameObject.layer==10)
        {
            if (positionsIndex < positions.Length - 1)
            {
                positionsIndex++;
            }
            else
            {
                positionsIndex = 0;
            }
        }
    }
}
