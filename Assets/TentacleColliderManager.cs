using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleColliderManager : MonoBehaviour
{
    public Rigidbody2D LeftShoulder;
    public Rigidbody2D RightShoulder;
    public Rigidbody2D Head;

    private List<Rigidbody2D> leftTentacleRBs;
    private List<Rigidbody2D> rightTentacleRBs;

    private EdgeCollider2D L_edgeCollider;
    private EdgeCollider2D R_edgeCollider;
    // Start is called before the first frame update
    void Start()
    {
        leftTentacleRBs = LeftShoulder.GetComponentsInChildren<Rigidbody2D>().ToList();
        rightTentacleRBs = RightShoulder.GetComponentsInChildren<Rigidbody2D>().ToList();
        L_edgeCollider = LeftShoulder.GetComponent<EdgeCollider2D>();
        R_edgeCollider = RightShoulder.GetComponent<EdgeCollider2D>();
    }

    private void FixedUpdate()
    {
        UpdateTentacleCollider(L_edgeCollider, leftTentacleRBs);
        UpdateTentacleCollider(R_edgeCollider, rightTentacleRBs);
    }

    private void UpdateTentacleCollider(EdgeCollider2D edgeCollider, List<Rigidbody2D> tentacleRBs)
    {
        Vector2[] points = new Vector2[tentacleRBs.Count];
        for (int i = 0; i < points.Length; i++)
        {
            // Convert the world position of the Rigidbody2D to the local space of the GameObject with the EdgeCollider2D
            points[i] = edgeCollider.transform.InverseTransformPoint(tentacleRBs[i].position);
        }
        // Update the edge collider points
        edgeCollider.SetPoints(new List<Vector2>(points));
    }
}
