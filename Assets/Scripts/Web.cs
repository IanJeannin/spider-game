using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float distanceBetweenNodes=2f;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private List<GameObject> nodes = new List<GameObject>();
    [SerializeField]
    private LineRenderer lineRenderer;
    [Tooltip("Multiplied by grapplePrefabs max grapple distance, player can move down web further than they can shoot web")]
    [SerializeField]
    private float maxWebDistanceModifier;

    private float maxAmountOfNodes;
    private Vector2 endPosition;
    private GameObject player;
    private GameObject lastNode;
    private bool done = false;
    private int vertexCount=2; //Two vertexes, hook and player

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastNode = transform.gameObject;
        nodes.Add(transform.gameObject);

        maxAmountOfNodes = (player.GetComponent<FireGrapple>().GetMaxGrappleDistance() *maxWebDistanceModifier)/distanceBetweenNodes;
    }

    private void FixedUpdate()
    {
       // GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = true;

        if (done==false&&(Vector2)transform.position != (Vector2)endPosition)
        {
            if(Vector2.Distance(player.transform.position, lastNode.transform.position)>distanceBetweenNodes)
            {
                CreateNode();
            }
        }
        else if(done == false)
        {
            done = true;

            //Ensures that once the end position is reached, nodes continue to create until they reach the player
            while(Vector2.Distance(player.transform.position, lastNode.transform.position) > distanceBetweenNodes)
            {
                CreateNode();
            }

            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }
        RenderLine();
       // GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
    }

    public void SetEndPosition(Vector2 newEndPosition)
    {
        endPosition = newEndPosition;
    }

    void RenderLine()
    {
        lineRenderer.positionCount = vertexCount;
        int x;
        for(x=0;x<nodes.Count;x++)
        {
            lineRenderer.SetPosition(x, nodes[x].transform.position);
        }
        lineRenderer.SetPosition(x, player.transform.position);
    }

    public void CreateNode()
    {
        //Finds the position from last node to player, sets the position of the new node to the last node times the distance between nodes in the direction of player
        Vector2 newNodePosition=player.transform.position-lastNode.transform.position;
        newNodePosition.Normalize();
        newNodePosition *= distanceBetweenNodes;
        newNodePosition += (Vector2)lastNode.transform.position;

        //Instantiates and creates a reference for the new node. Connects the two nodes and sets the new node as "lastNode" for the next iteration
        GameObject nodeReference = Instantiate(nodePrefab, newNodePosition, Quaternion.identity);
        nodeReference.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = nodeReference.GetComponent<Rigidbody2D>();
        lastNode = nodeReference;
        nodes.Add(lastNode);
        vertexCount++;
    }

    public void ChangePositionOnWeb(float verticalAxis)
    {
        if (done == true)
        {
            Debug.Log(nodes.Count);
            //For moving up: Set players position to closest node, remove and destroy that node, and connect the node after to the player
            if (verticalAxis > 0 && nodes.Count > 2)
            {
                player.transform.position = lastNode.transform.position;
                nodes.Remove(lastNode);
                Destroy(lastNode);
                vertexCount--;
                lastNode = nodes[nodes.Count - 1];
                nodes[nodes.Count - 1].GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            }
            else if(verticalAxis<0&&nodes.Count<maxAmountOfNodes)
            {
                    CreateNode();
                    //player.transform.position = lastNode.transform.position;
                    lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            }
        }
    }

}
