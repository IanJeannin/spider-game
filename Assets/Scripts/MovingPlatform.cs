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

    private void Start()
    {
        nextPos = positions[0].position;
    }

    private void Update()
    {
        if(transform.position==positions[positionsIndex].position)
        {
            if(positionsIndex<positions.Length-1)
            {
                positionsIndex++;
            }
            else
            {
                positionsIndex = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, positions[positionsIndex].position, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        for(int x=1;x<positions.Length;x++)
        {
            Gizmos.DrawLine(positions[x].position, positions[x-1].position);
        }
    }
}
