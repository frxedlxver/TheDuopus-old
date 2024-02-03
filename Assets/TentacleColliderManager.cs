using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TentacleColliderManager : MonoBehaviour
{
    public Rigidbody2D LeftShoulder;
    public Rigidbody2D RightShoulder;

    private List<Rigidbody2D> leftTentacleRBs;
    private List<Rigidbody2D> rightTentacleRBs;
    // Start is called before the first frame update
    void Start()
    {
        leftTentacleRBs = LeftShoulder.GetComponentsInChildren<Rigidbody2D>().ToList();
        leftTentacleRBs = LeftShoulder.GetComponentsInChildren<Rigidbody2D>().ToList();
    }

    private void Update()
    {
        UpdateTentacleCollider(LeftShoulder, leftTentacleRBs);
        UpdateTentacleCollider(RightShoulder, rightTentacleRBs);
    }

    private void UpdateTentacleCollider(Rigidbody2D leftShoulder, List<Rigidbody2D> leftTentacleRBs)
    {
        throw new NotImplementedException();
    }
}
